namespace Core.Models;

public class Result
{
    public int ErrCode { get; set; }
    public string ErrMsg { get; set; } = string.Empty;

    public bool IsSuccess => ErrCode == 0;

    public static Result Notfound => new()
    {
        ErrCode = 101,
        ErrMsg = "Not found resource!"
    };
}