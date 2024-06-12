namespace Maintenance.Core.Models;

public class Result
{
    public int ErrCode { get; set; } = 0;
    public string ErrMsg { get; set; } = string.Empty;

    public bool IsSuccess => ErrCode == 0;
}