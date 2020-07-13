using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ICS.Core.Dtos
{
    public class BaseEntity
    {
        /// <summary>
        /// Глобальный идентификатор парковочного места
        /// </summary>

        [Key]
        public Guid Guid { get; private set; }
        public DateTime CreatedAt { get; set; }
        public BaseEntity()
        {
            Guid = Guid.NewGuid();
            CreatedAt = DateTime.Now;
        }
    }
    /// <summary>
    /// Состояние парковки
    /// </summary>
    [Serializable]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ParkingCondition
    {
        /// <summary>
        /// Состояние простоя
        /// </summary>
        Idle,

        /// <summary>
        /// Состояние возврата средства передвижения
        /// </summary>
        InGetProcess,

        /// <summary>
        /// Состояние установки средства передвижения на место
        /// </summary>
        InPutProcess,

        /// <summary>
        /// Состояние сбоя
        /// </summary>
        Fault
    }
    /// <summary>
    /// Состояния парковочного места
    /// </summary>
    [Serializable]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ParkingPlaceCondition
    {
        ///// <summary>
        ///// Забронировано
        ///// </summary>
        //Reservation,

        /// <summary>
        /// Место свободно
        /// </summary>
        Free,

        /// <summary>
        /// Место занято
        /// </summary>
        Used,

        /// <summary>
        /// Место сломано
        /// </summary>
        Broken
    }
    /// <summary>
    /// Состояния сессии
    /// </summary>
    [Serializable]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SessionCondition
    {
        /// <summary>
        /// Бронирование
        /// </summary>
        Reservation,

        /// <summary>
        /// Велосипед припаркован
        /// </summary>
        Parked,

        /// <summary>
        /// Cессия завершена успешно
        /// </summary>
        Completed,

        /// <summary>
        /// Отменено по таймауту
        /// </summary>
        ReservationCanceledServer,

        /// <summary>
        /// Ошибка паркования
        /// </summary>
        ErrorParking,

        /// <summary>
        /// Ошибка возврата
        /// </summary>
        ReturnError,

        /// <summary>
        /// Отменено пользователем
        /// </summary>
        ReservationCanceledUser
    }

    /// <summary>
    /// Разрешение бронирования
    /// </summary>
    [Serializable]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ReservationAllowed
    {
        /// <summary>
        /// Запрещено
        /// </summary>
        Prohibited,

        /// <summary>
        /// Разрешено
        /// </summary>
        Allowed
    }

    /// <summary>
    /// Роли персонала
    /// </summary>
    [Serializable]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [Flags]
    public enum RolePersonnel
    {
        /// <summary>
        /// Нет назначенных ролей
        /// </summary>
        NoRoles = 1,
        /// <summary>
        /// Роль супервайзер
        /// </summary>
        Supervisor = 2,
        /// <summary>
        /// Роль администратор
        /// </summary>
        Administrator = 4,
        /// <summary>
        /// Роль инженер
        /// </summary>
        Engineer = 8
    }

    /// <summary>
    /// Состояния карты
    /// </summary>
    [Serializable]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [Flags]
    public enum CredentialCardCondition
    {
        /// <summary>
        /// Свободна
        /// </summary>
        Free,

        /// <summary>
        /// Используется
        /// </summary>
        Used
    }

    /// <summary>
    /// Состояние сервисной группы
    /// </summary>
    [Serializable]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [Flags]
    public enum ServiceGroupCondition
    {
        /// <summary>
        /// Используется
        /// </summary>
        Used,

        /// <summary>
        /// Удалена
        /// </summary>
        Delete
    }

    /// <summary>
    /// Вид юзера
    /// </summary>
    [Serializable]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [Flags]
    public enum TypeUser
    {
        /// <summary>
        /// Клиент
        /// </summary>
        Client,

        /// <summary>
        /// Работник
        /// </summary>
        Worker
    }
}
