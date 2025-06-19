using PerfectRandom.Sulfur.Core.UI;

namespace MenuLib;

public class Example {
    public static void AddButtonToMainMenu() {
        MenuAPI.AddButtonToMainMenu(parent => {
            MenuAPI.CreateMenuButton("Example Button", parent, () => {
                Plugin.Logger.LogInfo("Example Button clicked!");
            });
        });
    }
}