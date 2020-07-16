using ICS.Core.Dtos;

namespace ICS.Web.User.Localization
{
    public static class EnumLocalization
    {
        public static string LocalizationSessionCondition(SessionCondition condition)
        {
            switch (condition)
            {
                case SessionCondition.Reservation:
                    return "Место забронировано";
                case SessionCondition.Completed:
                    return "Велосипед возвращен";
                case SessionCondition.ErrorParking:
                    return "Ошибка паркования";
                case SessionCondition.Parked:
                    return "Велосипед припаркован";
                case SessionCondition.ReservationCanceledServer:
                    return "Бронирование отменено сервером";
                case SessionCondition.ReservationCanceledUser:
                    return "Бронирование отменено пользователем";
                case SessionCondition.ReturnError:
                    return "Ошибка возврата велосипеда";
                default:
                    return condition.ToString();
            }
        }
    }
}
