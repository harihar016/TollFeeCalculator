using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    public class TollCalculator
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public int GetTollFee(IVehicle vehicle, DateTime[] dates)
        {
            try
            {
                //Instead of calling IsTollFreeVehicle() method for all dates, check here only and remove this call from GetTollFee(dates)
                if (dates.Length == 0 || vehicle.IsTollFreeVehicle())
                    return 0;
                DateTime lastPaidTime = dates[0];
                int totalFee = GetTollFee(lastPaidTime);

                //Instead of hardcoding the maximum fees of 60 per day,take it from web.config so that we dont have to deploy our application in case if it changes in future.
                int maxFees = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxFeesPerDay"]);

                foreach (DateTime date in dates)
                {
                    //Its better to log these details to log files(using Log4Net dll) or to Database inorder to find the issues happened for which particular date in Production. 
                    logger.Info("calculation started for date " + date);

                    //Unnecessary two function call were there.Before calling function see if its paid paas or within 60 minutes filter.
                    //int nextFee = GetTollFee(date, vehicle);
                    //int tempFee = GetTollFee(intervalStart, vehicle);

                    //We can check minute difference instead of checking milliseconds difference to reduce lines of code
                    double minutes = (date - lastPaidTime).TotalMinutes;

                    //Instead of Hardcoding the 60 minute interval in the below code, take it from web.config so that we dont have to deploy our application in case if it changes in future.
                    int timeInterval = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TimeInterval"]);
                    if (minutes > timeInterval)
                    {
                        lastPaidTime = date; //Here lastPaidTime need to be reset with the current time to calculate the time interval for next time.
                        int curFee = GetTollFee(date); //Get fee for this trip.
                        totalFee += curFee;
                    }

                    //Break loop if bill cross 60. As it will be unnecesary waste of computation.
                    if (totalFee > maxFees)
                        break;
                }

                if (totalFee > maxFees) totalFee = maxFees;
                return totalFee;
            }
            catch (Exception ex)
            {
                //We should log this exception to db or any other log files in order to track the issues in production
                logger.Info("Exception in GetAadharDetails Method. Exception is " + ex.Message);
                throw ex;
            }
        }

        //We dont need this method as we are getting this value directly from Vehicle type classes

        //private bool IsTollFreeVehicle(IVehicle vehicle)
        //{
        //    if (vehicle == null) return false;
        //    String vehicleType = vehicle.GetVehicleType();

        //    return vehicleType.Equals(TollFreeVehicles.Motorbike.ToString()) ||
        //           vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
        //           vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
        //           vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
        //           vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
        //           vehicleType.Equals(TollFreeVehicles.Military.ToString());
        //}

        private int GetTollFee(DateTime date)
        {
            //if (IsTollFreeDate(date) || vehicle.IsTollFreeVehicle()) return 0;

            //Instead of calling IsTollFreeVehicle method here for all dates, check this before for loop
            if (IsTollFreeDate(date)) return 0;

            int hour = date.Hour;
            int minute = date.Minute;

            if (hour == 6 && minute >= 0 && minute <= 29) return 8;
            else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
            else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
            else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
            else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
            else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
            else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
            else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
            else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
            else return 0;
        }

        private Boolean IsTollFreeDate(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

            //This need revamp as everything is hardcoded.
            //We can have master holiday data and this will have to serve fast because lot of get request would come.We can store in Redis as SET data structure. Redis will remove unnecessary compution done below for each req.eg below
            //SADD holidayset “01012012”...
            if (year == 2018)
            {
                if (month == 1 && day == 1 ||
                    month == 3 && (day == 28 || day == 29) ||
                    month == 4 && (day == 1 || day == 30) ||
                    month == 5 && (day == 1 || day == 8 || day == 9) ||
                    month == 6 && (day == 5 || day == 6 || day == 21) ||
                    month == 7 ||
                    month == 11 && day == 1 ||
                    month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
                {
                    return true;
                }
            }
            return false;
        }

        //private enum TollFreeVehicles
        //{
        //    Motorbike = 0,
        //    Tractor = 1,
        //    Emergency = 2,
        //    Diplomat = 3,
        //    Foreign = 4,
        //    Military = 5
        //}
    }
}
