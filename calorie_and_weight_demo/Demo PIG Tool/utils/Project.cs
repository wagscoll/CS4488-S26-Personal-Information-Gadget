using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo_PIG_Tool.Utils;
namespace Demo_PIG_Tool.Utils
{
    //anh - Created a class to represent a project with its attributes and methods to update and retrieve those attributes.
    public class UtilsProject
    {
        private bool isImportant;
        private bool isUrgent;
        private DateTime dueDate;
        private float estimatedHours;
        private string projectName = "";
        private int projectId;

        public UtilsProject(int id, string name, bool isImportant, bool isUrgent, DateTime dueDate, float estimatedHours)
        {
            this.projectId = id;
            this.projectName = name;
            this.isImportant = isImportant;
            this.isUrgent = isUrgent;
            this.dueDate = dueDate;
            this.estimatedHours = estimatedHours;
        }

        public void updateName(string name)
        {
            this.projectName = name;
        }
        public void updateIsImportant(bool isImportant)
        {
            this.isImportant = isImportant;
        }
        public void updateIsUrgent(bool isUrgent)
        {
            this.isUrgent = isUrgent;
        }
        public void updateDueDate(DateTime dueDate)
        {
            this.dueDate = dueDate;
        }
        public void updateEstimatedHours(float estimatedHours)
        {
            this.estimatedHours = estimatedHours;
        }

        public int GetProjectId()
        {
            return this.projectId;
        }
        public string GetProjectName()
        {
            return this.projectName;
        }

        public bool getisImportant()
        {
            return this.isImportant;
        }
        public bool getisUrgent()
        {
            return this.isUrgent;
        }
        public DateTime getDueDate()
        {
            return this.dueDate;
        }
        public float getEstimatedHours()
        {
            return this.estimatedHours;
        }

    }
}
