using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo_PIG_Tool.Utils;
namespace Demo_PIG_Tool.Utils
{
    public class UtilsTask
    {
        private bool isImportant;
        private bool isUrgent;
        private DateTime dueDate;
        private float estimatedHours;
        private string taskName = "";
        private int taskId;
        private int projectId;

        public UtilsTask(int id, string taskName, bool isImportant, bool isUrgent, DateTime dueDate, float estimatedHours, int projectId)
        {
            this.taskId = id;
            this.taskName = taskName;
            this.isImportant = isImportant;
            this.isUrgent = isUrgent;
            this.dueDate = dueDate;
            this.estimatedHours = estimatedHours;
            this.projectId = projectId;
        }

        public void updateTaskName(string taskName)
        {
            this.taskName = taskName;
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
        public void updateProjectId(int projectId)
        {
            this.projectId = projectId;
        }

        public int GetTaskId()
        {
            return this.taskId;
        }

        public int getProjectId()
        {
            return this.projectId;
        }

        public string GetTaskName()
        {
            return this.taskName;
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