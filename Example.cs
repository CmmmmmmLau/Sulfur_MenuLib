namespace MenuLib;

public class Example {
    private enum ExampleEnum {
        Apple,
        Banana,
        Cherry
    }
    
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
            
            var dropdown = MenuAPI.CreateDropDown<string>("Example DropDown", ["Example 1", "Example 2", "Example 3"], "Example 2", "Example 1"
                , parent, s => {
                    Plugin.Logger.LogInfo($"Dropdown toggled: {s}");
            });
            
            var dropdown2 = MenuAPI.CreateDropDown<ExampleEnum>("Example DropDown", ExampleEnum.Apple, ExampleEnum.Banana
                , parent, s => {
                    Plugin.Logger.LogInfo($"Dropdown toggled: {s}");
                }, null);
        });
    }
}