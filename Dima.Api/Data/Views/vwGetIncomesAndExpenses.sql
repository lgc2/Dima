CREATE OR ALTER VIEW vwGetIncomesAndExpenses AS
SELECT
    t.UserId,
    MONTH(t.PaidOrReceivedAt) AS Month,
    YEAR(t.PaidOrReceivedAt) AS Year,
    SUM(CASE WHEN t.Type = 1 THEN t.Amount ELSE 0 END) AS Incomes,
    SUM(CASE WHEN t.Type = 2 THEN t.Amount ELSE 0 END) AS Expenses
FROM
    [Transaction] t
WHERE
    t.PaidOrReceivedAt >= DATEADD(MONTH, -11, CAST(GETDATE() AS DATE))
  AND t.PaidOrReceivedAt < DATEADD(MONTH, 1, CAST(GETDATE() AS DATE))
GROUP BY
    t.UserId,
    MONTH(t.PaidOrReceivedAt),
    YEAR(t.PaidOrReceivedAt);