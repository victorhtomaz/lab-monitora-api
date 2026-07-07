namespace MonitoraAgro.API.Application.Errors;

public static class ErrorMessages
{
    public const string UNKNOWN_ERROR = "Erro desconhecido.";
    public const string FIRST_NAME_REQUIRED = "O nome não pode estar vazio.";
    public const string LAST_NAME_REQUIRED = "O sobrenome não pode estar vazio.";
    public const string EMAIL_REQUIRED = "O email não pode estar vazio.";
    public const string PASSWORD_REQUIRED = "A senha não pode estar vazia.";
    public const string PASSWORD_MIN_LENGTH = "A senha deve conter no mínimo 8 caracteres";
    public const string INVALID_EMAIL = "O e-mail é inválido.";
    public const string EMAIL_ALREADY_REGISTERED = "Email já registrado";
    public const string INVALID_LOGIN = "Email ou senha inválidos.";
    public const string SENSOR_NOT_FOUND = "Sensor não encontrado.";
    public const string NOTIFICATION_TOKEN_REQUIRED = "O token não pode estar vazio.";
    public const string INSTALLATION_ID_REQUIRED = "O ID da instalação não pode estar vazio.";
    public const string DEVICE_NOT_FOUND = "Dispositivo não encontrado.";
    public const string NOTIFICATION_ENABLED_REQUIRED = "O campo de notificação deve ser definido como verdadeiro ou falso.";
    public const string LOWER_LIMIT_GREATER_THAN_UPPER_LIMIT = "O limite inferior não pode ser maior que o limite superior.";
}
