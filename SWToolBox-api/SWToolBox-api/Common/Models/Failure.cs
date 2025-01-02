namespace SWToolBox_api.Common.Models;

public readonly struct Failure(string errorMessage)
{
    public string ErrorMessage => errorMessage;
};