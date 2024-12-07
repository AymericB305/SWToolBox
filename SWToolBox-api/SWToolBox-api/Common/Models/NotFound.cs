namespace SWToolBox_api.Common.Models;

public readonly struct NotFound(string errorMessage)
{
    public string ErrorMessage => errorMessage;
}