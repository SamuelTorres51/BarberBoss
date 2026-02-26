namespace BarberBoss.Communication.Responses;

public class ResponseErrorsJson {
    public List<string> Errors { get; set; }

    public ResponseErrorsJson(string errorMessage) {
        Errors = [errorMessage];
    }

    public ResponseErrorsJson(List<string> errorMessages) {
        Errors = errorMessages;
    }
}
