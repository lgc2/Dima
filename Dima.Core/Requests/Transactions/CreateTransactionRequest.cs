﻿using System.ComponentModel.DataAnnotations;
using Dima.Core.Enums;

namespace Dima.Core.Requests.Transactions;

public class CreateTransactionRequest : Request
{
	[Required(ErrorMessage = "Invalid title")]
	public string Title { get; set; } = string.Empty;

	[Required(ErrorMessage = "Invalid type")]
	public ETransactionType Type { get; set; } = ETransactionType.Withdraw;

	[Required(ErrorMessage = "Invalid amount")]
	public decimal Amount { get; set; }

	[Required(ErrorMessage = "Invalid category")]
	public long CategoryId { get; set; }

	[Required(ErrorMessage = "Invalid date")]
	public DateTime? PaidOrReceivedAt { get; set; }
}
