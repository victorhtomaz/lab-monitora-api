using System.Runtime.Serialization;

namespace MonitoraAgro.API.Domain.Enums;

public enum AvailablePeriod
{
    [EnumMember(Value = "1d")]
    OneDay,

    [EnumMember(Value = "1w")]
    OneWeek,

    [EnumMember(Value = "1m")]
    OneMonth,

    [EnumMember(Value = "1s")]
    OneSemester
}
