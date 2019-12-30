using System;

namespace IrbImgFormat
{
    public class Logging
    {
        private string m_className;

        //- main error handler
        private static Logging s_mainError;


        //-------------------------------------------------------//
        public enum enumErrorType
        {
            enumErrorType_error,
            enumErrorType_warning,
            enumErrorType_info
        }


        //-------------------------------------------------------//
        public Logging(string classStr = "")
        {
            m_className = classStr;
        }

        //-------------------------------------------------------//
        public bool addError(string errorStr, string info1 = null, string info2 = null, string info3 = null)
        {
            getMainInstance().addMSG(errorStr, enumErrorType.enumErrorType_error, m_className, info1, info2, info3);
            return false;
        }

        //-------------------------------------------------------//
        public bool addInfo(string errorStr, string info1 = null, string info2 = null, string info3 = null)
        {
            getMainInstance().addMSG(errorStr, enumErrorType.enumErrorType_info, m_className, info1, info2, info3);
            return false;
        }

        //-------------------------------------------------------//
        public bool addWarning(string errorStr, string info1 = null, string info2 = null, string info3 = null)
        {
            getMainInstance().addMSG(errorStr, enumErrorType.enumErrorType_warning, m_className, info1, info2, info3);
            return false;
        }

        //-------------------------------------------------------//
        public static void addAnonymousError(string errorStr, string className = "", string info1 = null, string info2 = null, string info3 = null)
        {
            getMainInstance().addMSG(errorStr, enumErrorType.enumErrorType_error, className, info1, info2, info3);
        }

        //-------------------------------------------------------//
        protected static Logging getMainInstance()
        {
            if (s_mainError == null) s_mainError = new Logging("");

            return s_mainError;
        }




        //-------------------------------------------------------//
        protected void addMSG(string errorStr, enumErrorType ErrorType, string classStr, string errorStr2 = null, string errorStr3 = null, string errorStr4 = null)
        {
            System.Diagnostics.Debug.WriteLine(classStr + " " + errorStr + " " + errorStr2);

            System.DateTime ErrorTimeStamp = System.DateTime.Now;

            System.Console.WriteLine(ErrorType + "/t" + ErrorTimeStamp + "/t" + errorStr + "/t" + classStr + "/t" + errorStr2 + "/t" + errorStr3 + "/t" + errorStr4);

        }


    }

}
