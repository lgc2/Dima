from __future__ import annotations

import html
import sys
import xml.etree.ElementTree as ET
from pathlib import Path


def get_namespace(root_tag: str) -> str | None:
    if root_tag.startswith("{"):
        return root_tag[1: root_tag.index("}")]
    return None


def qname(tag: str, namespace: str | None) -> str:
    return f"{{{namespace}}}{tag}" if namespace else tag


def render_html(summary: dict[str, str | int], results: list[dict[str, str]], output_path: Path) -> None:
    output_path.parent.mkdir(parents=True, exist_ok=True)
    title = "Integration Test Results"
    with output_path.open("w", encoding="utf-8") as f:
        f.write("""<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<title>Integration Test Results</title>
<style>body{font-family:Arial,Helvetica,sans-serif;background:#f7f7f7;color:#222;margin:0;padding:0}header{padding:20px 24px;background:#1e3a8a;color:white}main{padding:24px}h1{margin:0 0 8px}table{border-collapse:collapse;width:100%;margin-top:16px;background:white}th,td{padding:12px 10px;border:1px solid #ddd;text-align:left}th{background:#f3f4f6}tr:nth-child(even){background:#f9fafb}.status-pass{color:#047857;font-weight:700}.status-fail{color:#b91c1c;font-weight:700}.status-other{color:#92400e;font-weight:700}.summary-card{display:inline-block;background:white;padding:16px 18px;margin:8px 8px 8px 0;border:1px solid #e5e7eb;border-radius:8px;min-width:180px}</style>
</head>
<body>
<header>
<h1>Integration Test Results</h1>
</header>
<main>
""")
        f.write("<div>")
        for label, value in summary.items():
            f.write(f"<div class=\"summary-card\"><strong>{html.escape(label)}</strong><div>{html.escape(str(value))}</div></div>")
        f.write("</div>\n")
        if not results:
            f.write("<p>No tests were found in the TRX file.</p>\n")
        else:
            f.write("<table>\n<tr><th>Test</th><th>Outcome</th><th>Duration</th><th>Message</th></tr>\n")
            for result in results:
                cls = "status-pass" if result["outcome"] == "Passed" else "status-fail" if result["outcome"] in {"Failed", "Error"} else "status-other"
                f.write(
                    "<tr>"
                    f"<td>{html.escape(result['test_name'])}</td>"
                    f"<td class=\"{cls}\">{html.escape(result['outcome'])}</td>"
                    f"<td>{html.escape(result['duration'])}</td>"
                    f"<td>{html.escape(result['message'])}</td>"
                    "</tr>\n"
                )
            f.write("</table>\n")
        f.write("</main>\n</body>\n</html>\n")


def parse_trx(trx_path: Path) -> tuple[dict[str, str | int], list[dict[str, str]]]:
    tree = ET.parse(trx_path)
    root = tree.getroot()
    namespace = get_namespace(root.tag)

    counters = root.find(f".//{qname('Counters', namespace)}")
    summary: dict[str, str | int] = {}
    if counters is not None:
        attrs = counters.attrib
        summary = {
            'Total': attrs.get('total', '0'),
            'Executed': attrs.get('executed', '0'),
            'Passed': attrs.get('passed', '0'),
            'Failed': attrs.get('failed', '0'),
            'Error': attrs.get('error', '0'),
            'Timeout': attrs.get('timeout', '0'),
            'Aborted': attrs.get('aborted', '0'),
            'NotExecuted': attrs.get('notExecuted', '0'),
        }

    results: list[dict[str, str]] = []
    for node in root.findall(f".//{qname('UnitTestResult', namespace)}"):
        test_name = node.attrib.get('testName', '')
        outcome = node.attrib.get('outcome', '')
        duration = node.attrib.get('duration', '')
        msg = ''
        output = node.find(f".//{qname('Output', namespace)}")
        if output is not None:
            error_info = output.find(f".//{qname('ErrorInfo', namespace)}")
            if error_info is not None:
                message_node = error_info.find(f"{qname('Message', namespace)}")
                if message_node is not None and message_node.text:
                    msg = message_node.text.strip()
        results.append({
            'test_name': test_name,
            'outcome': outcome,
            'duration': duration,
            'message': msg,
        })

    return summary, results


def main(argv: list[str] | None = None) -> int:
    argv = argv or sys.argv
    if len(argv) != 3:
        sys.stderr.write('Usage: python generate-test-results-html.py <input.trx> <output.html>\n')
        return 1

    input_path = Path(argv[1])
    output_path = Path(argv[2])
    if not input_path.exists():
        sys.stderr.write(f'TRX file not found: {input_path}\n')
        return 2

    summary, results = parse_trx(input_path)
    render_html(summary, results, output_path)
    return 0


if __name__ == '__main__':
    raise SystemExit(main())
