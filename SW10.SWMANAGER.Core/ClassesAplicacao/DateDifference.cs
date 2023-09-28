using System;

namespace SW10.SWMANAGER.ClassesAplicacao
{
    public static class DateDifference
    {
        public static Idade GetExtendedDifference(DateTime dt)
        {
            int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            DateTime fromDate;
            DateTime toDate;
            int year;
            int month;
            int day;
            DateTime d1, d2;
            d1 = dt;
            d2 = DateTime.UtcNow;
            var dateOfBirth = d1.Day.ToString() + "/" + d1.Month.ToString();
            var today = d2.Day.ToString() + "/" + d2.Month.ToString();
            var message = dateOfBirth.Equals(today) ? " - Parabéns!" : string.Empty;
            if (d1 > d2)
            {
                fromDate = d2;
                toDate = d1;
            }
            else
            {
                fromDate = d1;
                toDate = d2;
            }
            var increment = 0;
            if (fromDate.Day > toDate.Day)
            {
                increment = monthDay[fromDate.Month - 1];
            }
            if (increment == -1)
            {
                if (DateTime.IsLeapYear(fromDate.Year))
                {
                    increment = 29;
                }
                else
                {
                    increment = 28;
                }
            }
            if (increment != 0)
            {
                day = (toDate.Day + increment) - fromDate.Day;
                increment = 1;
            }
            else
            {
                day = toDate.Day - fromDate.Day;
            }
            if ((fromDate.Month + increment) > toDate.Month)
            {
                month = (toDate.Month + 12) - (fromDate.Month + increment);
                increment = 1;
            }
            else
            {
                month = (toDate.Month) - (fromDate.Month + increment);
                increment = 0;
            }
            year = toDate.Year - (fromDate.Year + increment);

            var idade = new Idade();
            idade.Ano = year;
            idade.Mes = month;
            idade.Dia = day;
            idade.Mensagem = message;
            return idade;
        }



        public static Idade GetExtendedDifference(DateTime dt, DateTime dataOrigem)
        {
            int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            DateTime fromDate;
            DateTime toDate;
            int year;
            int month;
            int day;
            DateTime d1, d2;
            d1 = dt;
            d2 = dataOrigem;
            var dateOfBirth = d1.Day.ToString() + "/" + d1.Month.ToString();
            var today = d2.Day.ToString() + "/" + d2.Month.ToString();
            // var message = dateOfBirth.Equals(today) ? " - Parabéns!" : string.Empty;
            if (d1 > d2)
            {
                fromDate = d2;
                toDate = d1;
            }
            else
            {
                fromDate = d1;
                toDate = d2;
            }
            var increment = 0;
            if (fromDate.Day > toDate.Day)
            {
                increment = monthDay[fromDate.Month - 1];
            }
            if (increment == -1)
            {
                if (DateTime.IsLeapYear(fromDate.Year))
                {
                    increment = 29;
                }
                else
                {
                    increment = 28;
                }
            }
            if (increment != 0)
            {
                day = (toDate.Day + increment) - fromDate.Day;
                increment = 1;
            }
            else
            {
                day = toDate.Day - fromDate.Day;
            }
            if ((fromDate.Month + increment) > toDate.Month)
            {
                month = (toDate.Month + 12) - (fromDate.Month + increment);
                increment = 1;
            }
            else
            {
                month = (toDate.Month) - (fromDate.Month + increment);
                increment = 0;
            }
            year = toDate.Year - (fromDate.Year + increment);

            var idade = new Idade();
            idade.Ano = year;
            idade.Mes = month;
            idade.Dia = day;
            //   idade.Mensagem = message;
            return idade;
        }

    }

}


