namespace MenuLib;

public class Example {
    public static void AddExample() {
        string categoryName = "Example Category 1";
        MenuAPI.AddNewCategory(categoryName, () => {
            Plugin.Logger.LogInfo("Example Category clicked!");
        });
        
        categoryName = "Example Category 2";
        MenuAPI.AddNewCategory(categoryName, (parent) => {
            var toggle = MenuAPI.CreateCheckBox("Example CheckBox", true, false, parent, (value) => {
                Plugin.Logger.LogInfo($"Checkbox toggled: {value}");
            });
        });
        
        categoryName = "Example Category 3";
        MenuAPI.AddNewCategory(categoryName, (parent) => {
            var toggle = MenuAPI.CreateCheckBox("Example CheckBox 2", false, true, parent, (value) => {
                Plugin.Logger.LogInfo($"Checkbox toggled: {value}");
            });
        });
    }
}