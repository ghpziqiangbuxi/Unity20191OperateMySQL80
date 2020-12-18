namespace Assets.Scripts.Data
{
    public class SpliceRecord
    {
        public string qrCode; 
        public string batteryType;
        public string scanStartTime;
        public string scanEndTime;
        public string checkConfig;
        public string wasteLine;
        public string defectArea;
        public string defectAreaRate;
        public string isQualified;
        public string saveDataName;

        public void Init()
        {
            qrCode = "123456789";
            batteryType = "666";
            scanStartTime = "20201231000000";
            scanEndTime = "20201231000000";
            checkConfig = "config";
            wasteLine = "20%";
            defectArea = "200";
            defectAreaRate = "10%";
            isQualified = "合格";
            saveDataName = "datafilename";
        }
    }
}