namespace BarberBoss.Exception.ExceptionBase;

public class ErrorOnValidatorException : BarberBossException{
    public List<string> Errors { get; set; }

    public ErrorOnValidatorException(List<string> errors) : base(string.Empty) {
        Errors = errors;
    }
}
