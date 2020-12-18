using Assets.Scripts.Data;
using Assets.Scripts.MySql;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts.View
{
    public class OperateDbView : MonoBehaviour
    {
        private Text m_Text;
        private InputField m_InputField;
        private Button m_BtnCreateDb;
        private Button m_BtnCreateTable;
        private Button m_BtnInsert;
        private Button m_BtnSelect;
        private string m_TableName;
        private MySqlModel m_MySqlModel;

        void Start()
        {
            m_Text = transform.Find("Text").GetComponent<Text>();
            m_InputField = transform.Find("InputField").GetComponent<InputField>();
            m_InputField.onEndEdit.AddListener(OnInputFieldonEndEdit);

            m_BtnCreateDb = transform.Find("Grid/CreateDb").GetComponent<Button>();
            m_BtnCreateDb.onClick.AddListener(OnBtnCreateDb);

            m_BtnCreateTable = transform.Find("Grid/CreateTable").GetComponent<Button>();
            m_BtnCreateTable.onClick.AddListener(OnBtnCreateTable);
        
            m_BtnInsert = transform.Find("Grid/Insert").GetComponent<Button>();
            m_BtnInsert.onClick.AddListener(OnBtnInsert);
            m_BtnSelect = transform.Find("Grid/Select").GetComponent<Button>();
            m_BtnSelect.onClick.AddListener(OnBtnSelect);
            InitValue();
        }

        private void InitValue()
        {
            m_TableName = "";
            m_InputField.text = m_TableName;
            m_MySqlModel = MySqlModel.Instance;
        }

        private void OnInputFieldonEndEdit(string arg)
        {
            m_TableName = m_InputField.text;
        }

        private void OnBtnCreateDb()
        {
            //m_MySqlModel.CreateLocalDbIfNotExsit();
            m_MySqlModel.CreateRemoteDbIfNotExsit();
        }

        private void OnBtnCreateTable()
        {
            SpliceRecord spliceRecord = new SpliceRecord();
            spliceRecord.Init();
            //m_MySqlModel.CreateLocalTableIfNotExsit(sr);
            m_MySqlModel.CreateRemoteTableIfNotExsit(spliceRecord);
        }
        
        private void OnBtnInsert()
        {
            SpliceRecord spliceRecord = new SpliceRecord();
            spliceRecord.Init();
            //m_MySqlModel.LocalInsert(sr);
            m_MySqlModel.RemoteInsert(spliceRecord);
        }

        private void OnBtnSelect()
        {
        }
    }
}
