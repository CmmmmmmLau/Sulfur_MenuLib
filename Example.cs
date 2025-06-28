using PerfectRandom.Sulfur.Core.UI;

namespace MenuLib;

public class Example {
    public static void AddExample() {
        MenuAPI.AddNewCategory(parent => {
            MenuAPI.CreateNewCategoryButton("Example Category 1", parent, () => {
                Plugin.Logger.LogInfo("Example Category clicked!");
            });
            
            MenuAPI.CreateNewCategoryButton("Example Category 2", parent, null);
        });
    }
}