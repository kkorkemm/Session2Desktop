using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session2Desktop.Base
{
    /// <summary>
    /// Класс для получения контекста данных
    /// </summary>
    public class AppData
    {
        private static KazanNeftSession2DBEntities context;

        /// <summary>
        /// Получение модели базы данных
        /// </summary>
        /// <returns>Контекст данных</returns>
        public static KazanNeftSession2DBEntities GetContext()
        {
            if (context == null)
                context = new KazanNeftSession2DBEntities();
            return context;
        }

        /// <summary>
        /// Текущий пользователь системы
        /// </summary>
        public static Employees CurrentEmployee { get; set; }
    }
}
