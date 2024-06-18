namespace Maintenance.Core.Models;

public class Result
{
    public int ErrCode { get; set; } = 0;
    public string ErrMsg { get; set; } = string.Empty;

    public bool IsSuccess => ErrCode == 0;

    public static Result Notfound => new Result
    {
        ErrCode = 101,
        ErrMsg = "Not found resource!"
    };
}