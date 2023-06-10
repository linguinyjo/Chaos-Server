using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Chaos.Common.Definitions;
using Chaos.Common.Utilities;
using Chaos.Extensions.Common;
using Chaos.Schemas.Aisling;
using Chaos.Schemas.Data;
using Chaos.Schemas.Templates;
using ChaosTool.Model;

namespace ChaosTool.Controls.SkillControls;

public sealed partial class SkillPropertyEditor
{
    public ObservableCollection<ItemRequirementSchema> ItemRequirementsViewItems { get; }
    public ListViewItem<SkillTemplateSchema, SkillPropertyEditor> ListItem { get; }
    public ObservableCollection<string> PrereqSkillTemplateKeysViewItems { get; }
    public ObservableCollection<string> PrereqSpellTemplateKeysViewItems { get; }
    public ObservableCollection<string> ScriptKeysViewItems { get; }

    public TraceWrapper<SkillTemplateSchema> Wrapper => ListItem.Wrapper;

    public SkillPropertyEditor(ListViewItem<SkillTemplateSchema, SkillPropertyEditor> listItem)
    {
        ListItem = listItem;
        ScriptKeysViewItems = new ObservableCollection<string>();
        ItemRequirementsViewItems = new ObservableCollection<ItemRequirementSchema>();
        PrereqSpellTemplateKeysViewItems = new ObservableCollection<string>();
        PrereqSkillTemplateKeysViewItems = new ObservableCollection<string>();

        InitializeComponent();
    }

    private void UserControl_Initialized(object sender, EventArgs e)
    {
        ClassCmbox.ItemsSource = GetEnumNames<BaseClass?>();
        AdvClassCmbox.ItemsSource = GetEnumNames<AdvClass?>();

        ScriptKeysView.ItemsSource = ScriptKeysViewItems;
        ItemRequirementsView.ItemsSource = ItemRequirementsViewItems;
        PrereqSpellTemplateKeysView.ItemsSource = PrereqSpellTemplateKeysViewItems;
        PrereqSkillTemplateKeysView.ItemsSource = PrereqSkillTemplateKeysViewItems;

        PopulateControlsFromItem();
    }

    #region Controls > Template > Controls
    public void CopySelectionsToItem()
    {
        var template = Wrapper.Object;
        var learningRequirements = new LearningRequirementsSchema();
        var stats = new StatsSchema();

        Wrapper.Path = PathTbox.Text;
        template.TemplateKey = TemplateKeyTbox.Text;
        template.Name = NameTbox.Text;
        template.IsAssail = IsAssailCbox?.IsChecked ?? false;
        template.PanelSprite = ParsePrimitive<ushort>(PanelSpriteTbox.Text);
        template.Level = ParsePrimitive<int>(LevelTbox.Text);
        template.Class = ParsePrimitive<BaseClass?>(ClassCmbox.Text);
        template.AdvClass = ParsePrimitive<AdvClass?>(AdvClassCmbox.Text);
        template.RequiresMaster = RequiresMasterCbox.IsChecked ?? false;

        learningRequirements.RequiredGold = ParsePrimitive<int?>(RequiredGoldTbox.Text);
        stats.Str = ParsePrimitive<int>(StrTbox.Text);
        stats.Int = ParsePrimitive<int>(IntTbox.Text);
        stats.Wis = ParsePrimitive<int>(WisTbox.Text);
        stats.Con = ParsePrimitive<int>(ConTbox.Text);
        stats.Dex = ParsePrimitive<int>(DexTbox.Text);

        learningRequirements.ItemRequirements = ItemRequirementsViewItems.Select(ShallowCopy<ItemRequirementSchema>.Create).ToList();
        learningRequirements.PrerequisiteSpellTemplateKeys = PrereqSpellTemplateKeysViewItems.ToList();
        learningRequirements.PrerequisiteSkillTemplateKeys = PrereqSkillTemplateKeysViewItems.ToList();

        template.CooldownMs = ParsePrimitive<int?>(CooldownMsTbox.Text);
        template.Description = DescriptionTbox.Text;
        template.ScriptKeys = ScriptKeysViewItems.ToList();

        ListItem.Name = template.TemplateKey;

        if (!stats.Equals(new StatsSchema()))
            learningRequirements.RequiredStats = stats;

        if (!learningRequirements.Equals(new LearningRequirementsSchema()))
            template.LearningRequirements = learningRequirements;
    }

    public void PopulateControlsFromItem()
    {
        var template = Wrapper.Object;
        var learningRequirements = template.LearningRequirements;
        var stats = learningRequirements?.RequiredStats;

        PathTbox.Text = Wrapper.Path;
        TemplateKeyTbox.Text = template.TemplateKey;

        NameTbox.Text = template.Name;
        IsAssailCbox.IsChecked = template.IsAssail;
        PanelSpriteTbox.Text = template.PanelSprite.ToString();
        LevelTbox.Text = template.Level.ToString();
        ClassCmbox.SelectedItem = SelectPrimitive(template.Class, ClassCmbox.Items);
        AdvClassCmbox.SelectedItem = SelectPrimitive(template.AdvClass, AdvClassCmbox.Items);
        RequiresMasterCbox.IsChecked = template.RequiresMaster;
        RequiredGoldTbox.Text = learningRequirements?.RequiredGold.ToString();
        StrTbox.Text = stats?.Str.ToString();
        IntTbox.Text = stats?.Int.ToString();
        WisTbox.Text = stats?.Wis.ToString();
        ConTbox.Text = stats?.Con.ToString();
        DexTbox.Text = stats?.Dex.ToString();
        CooldownMsTbox.Text = template.CooldownMs.ToString();
        DescriptionTbox.Text = template.Description;

        ItemRequirementsViewItems.Clear();

        ItemRequirementsViewItems.AddRange(
            learningRequirements?.ItemRequirements.Select(ShallowCopy<ItemRequirementSchema>.Create).ToList()
            ?? new List<ItemRequirementSchema>());

        PrereqSpellTemplateKeysViewItems.Clear();
        PrereqSpellTemplateKeysViewItems.AddRange(learningRequirements?.PrerequisiteSpellTemplateKeys.ToList() ?? new List<string>());

        PrereqSkillTemplateKeysViewItems.Clear();
        PrereqSkillTemplateKeysViewItems.AddRange(learningRequirements?.PrerequisiteSkillTemplateKeys.ToList() ?? new List<string>());

        ScriptKeysViewItems.Clear();
        ScriptKeysViewItems.AddRange(template.ScriptKeys.ToList());
    }
    #endregion

    #region Buttons
    private void RevertBtn_Click(object sender, RoutedEventArgs e) => PopulateControlsFromItem();

    private async void SaveBtn_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var existing = JsonContext.SkillTemplates.Objects.Where(obj => obj != Wrapper)
                                      .FirstOrDefault(obj => obj.Path == Wrapper.Path);

            if (existing is not null)
            {
                Snackbar.MessageQueue?.Enqueue($"Save failed. An item already exists at path \"{existing.Path}\"");

                return;
            }

            existing = JsonContext.SkillTemplates.Objects.Where(obj => obj != Wrapper)
                                  .FirstOrDefault(obj => obj.Object.TemplateKey.EqualsI(Wrapper.Object.TemplateKey));

            if (existing is not null)
            {
                Snackbar.MessageQueue?.Enqueue(
                    $"Save failed. An item already exists with template key \"{existing.Object.TemplateKey}\" at path \"{existing.Path}\"");

                return;
            }

            existing = JsonContext.SkillTemplates.Objects.FirstOrDefault(obj => ReferenceEquals(obj, Wrapper));

            if (existing is null)
                JsonContext.SkillTemplates.Objects.Add(Wrapper);

            if (!ValidatePreSave(Wrapper, PathTbox, TemplateKeyTbox))
            {
                Snackbar.MessageQueue?.Enqueue("Filename does not match the template key");

                return;
            }

            CopySelectionsToItem();
            PopulateControlsFromItem();
        } catch (Exception ex)
        {
            Snackbar.MessageQueue?.Enqueue(ex.ToString());
        }

        await JsonContext.SkillTemplates.SaveItemAsync(Wrapper);
    }
    #endregion

    #region Tbox Validation
    private void TboxNumberValidator(object sender, TextCompositionEventArgs e) => Validators.NumberValidationTextBox(sender, e);
    private void TemplateKeyTbox_OnKeyUp(object sender, KeyEventArgs e) => Validators.TemplateKeyMatchesFileName(TemplateKeyTbox, PathTbox);
    #endregion

    #region ScriptKeys Controls
    private void AddScriptKeyBtn_Click(object sender, RoutedEventArgs e) => ScriptKeysViewItems.Add(string.Empty);

    private void DeleteScriptKeyBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
            return;

        if (button.DataContext is not string scriptKey)
            return;

        ScriptKeysViewItems.Remove(scriptKey);
    }
    #endregion

    #region PrereqSpellTemplateKeys Controls
    private void DeleteSkillTemplateKeyBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
            return;

        if (button.DataContext is not string skillTemplateKey)
            return;

        PrereqSkillTemplateKeysViewItems.Remove(skillTemplateKey);
    }

    private void AddSkillTemplateKeyBtn_Click(object sender, RoutedEventArgs e) => PrereqSkillTemplateKeysViewItems.Add(string.Empty);
    #endregion

    #region PrereqSpellTemplateKeys Controls
    private void DeleteSpellTemplateKeyBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
            return;

        if (button.DataContext is not string skillTemplateKey)
            return;

        PrereqSpellTemplateKeysViewItems.Remove(skillTemplateKey);
    }

    private void AddSpellTemplateKeyBtn_Click(object sender, RoutedEventArgs e) => PrereqSpellTemplateKeysViewItems.Add(string.Empty);
    #endregion

    #region ItemRequirements Controls
    private void DeleteItemRequirementBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
            return;

        if (button.DataContext is not ItemRequirementSchema itemRequirement)
            return;

        ItemRequirementsViewItems.Remove(itemRequirement);
    }

    private void AddItemRequirementBtn_Click(object sender, RoutedEventArgs e) =>
        ItemRequirementsViewItems.Add(new ItemRequirementSchema());
    #endregion
}