using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace StudentsDiary
{
    public partial class AddEditStudent : Form
    {
        private int _studentId;
        private Student _student;
        private List<Group> _groups;

        private FileHelper<List<Student>> _fileHelper =
            new FileHelper<List<Student>>(Program.FilePath);
        public AddEditStudent(int id = 0)
        {
            InitializeComponent();
            _studentId = id;
            if (id != 0)

                _groups = GroupsHelper.GetGroups("Brak");

                InitGroupsCombobox();
                GetStudentData();
            tbFirstName.Select();
        }

        private void InitGroupsCombobox()
        {
            cmbGroup.DataSource = _groups;
            cmbGroup.DisplayMember = "Name";
            cmbGroup.ValueMember = "Id";
        }

        private void GetStudentData()
        {
            if (_studentId != 0)
            {
                Text = "Edytowanie danych ucznia";
                var students = _fileHelper.DeserializeFromFile();
                _student = students.FirstOrDefault(x => x.Id == _studentId);

                if (_student == null)
                    throw new Exception("Brak użytkownika o podanym Id");
                FillTextBoxes();
            }
        }

        private void FillTextBoxes()
        {
            tbID.Text = _student.Id.ToString();
            tbFirstName.Text = _student.FirstName;
            tbLastName.Text = _student.LastName;
            rtbComments.Text = _student.Comments;
            tbMath.Text = _student.Math;
            tbTechnology.Text = _student.Technology;
            tbPhysics.Text = _student.Physics;
            tbPolishLang.Text = _student.PolishLang;
            tbForeignLang.Text = _student.ForeignLang;
            cbAdditionalClasses.Checked = _student.AdditionalClasses;
            cmbGroup.SelectedItem = _groups.FirstOrDefault(x => x.Id == _student.GroupId);
        }

        

             private void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromFile();

            if (_studentId != 0)
            {
                students.RemoveAll(x => x.Id == _studentId);
            }
            else
                AsignIdToNewStudent(students);

            AddNewUserToList(students);

            _fileHelper.SerializeToFile(students);

            Close();
        }


        private void AddNewUserToList(List<Student>students)
        {
            var student = new Student
            {
                Id = _studentId,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                Comments = rtbComments.Text,
                Math = tbMath.Text,
                Technology = tbTechnology.Text,
                Physics = tbPhysics.Text,
                PolishLang = tbPolishLang.Text,
                ForeignLang = tbForeignLang.Text,
                AdditionalClasses = cbAdditionalClasses.Checked,
                GroupId = (cmbGroup.SelectedItem as Group).Id
            };

            students.Add(student);
        }

        private void AsignIdToNewStudent(List<Student>students)
        {
            var studentWithHighestId = students.OrderByDescending(x => x.Id).FirstOrDefault();

            _studentId = studentWithHighestId == null ? 1 : studentWithHighestId.Id + 1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddEditStudent_Load(object sender, EventArgs e)
        {

        }

        private void cbClassId_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
