using System.Collections.Generic;
using TaskCards.Dao;
using TaskCards.Entities;

namespace TaskCards.ViewModels {

	/// <summary>
	/// タスクページのビューモデル
	/// </summary>
	public class TaskViewModel {

		public const double ProjectTitleHeightRate = 0.05;
		public const double TaskTitleHeightRate = 0.06;
		public const double TaskProgressHeightRate = 0.06;
		public const double MarginVerticalRate = 0.03;

		public double DialogBaseHeight { get; set; }

		public double marginBottom = 0;

		public TaskViewModel(double height) {

			double dialogBaseHeight = 0;

			// TODO 仮のメンバーを初期選択に
			long memberId = 1;

			// プロジェクト・タスク数からレイアウト全体の高さを計算する。
			var projectDao = new ProjectDao();
			List<Project> projectList = projectDao.GetProjectListByMemberId(memberId);

			var taskDao = new TaskDao();

			foreach (Project entity in projectList) {
				List<Task> taskList = taskDao.GetTaskListByProjectId(entity.Id);
				dialogBaseHeight += GetProjectGridHeight(height, taskList.Count) + height * MarginVerticalRate;
			}

			// 画面の高さに満たない場合、最下部の空白の高さを設定する。
			if (dialogBaseHeight < height) this.marginBottom = height - dialogBaseHeight;

			// レイアウト全体の高さの設定
			DialogBaseHeight = dialogBaseHeight;
		}

		/// <summary>
		/// プロジェクトグリッドの高さを取得する。
		/// </summary>
		/// <param name="height">画面の高さ</param>
		/// <param name="taskCount">タスク数</param>
		/// <returns>高さ</returns>
		public double GetProjectGridHeight(double height, int taskCount) {
			return height * ProjectTitleHeightRate + GetTasksGridHeight(height, taskCount);
		}

		/// <summary>
		/// タスク群グリッドの高さを取得する。
		/// </summary>
		/// <param name="height">画面の高さ</param>
		/// <param name="taskCount">タスク数</param>
		/// <returns>高さ</returns>
		public double GetTasksGridHeight(double height, int taskCount) {

			double tasksGridHeight = height * MarginVerticalRate;

			if (taskCount == 0) {
				tasksGridHeight += height * TaskTitleHeightRate;
			}
			else {
				tasksGridHeight += height * (TaskTitleHeightRate + TaskProgressHeightRate) * taskCount;
			}

			return tasksGridHeight;
		}
	}
}