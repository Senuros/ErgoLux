﻿using System.Globalization;
using System.Linq;

namespace ErgoLux;

public partial class FrmLanguage : Form
{
    private CultureInfo _culture = CultureInfo.CurrentCulture;
    private readonly ClassSettings? Settings;

    public FrmLanguage()
    {
        InitializeComponent();
        FillDefinedCultures("ErgoLux.localization.strings", typeof(FrmMain).Assembly);
    }

    public FrmLanguage(ClassSettings settings)
    : this()
    {
        Settings = settings;
        _culture = settings.AppCulture;
        UpdateControls(settings.AppCultureName);
    }

    private void Accept_Click(object sender, EventArgs e)
    {
        if (Settings is not null) Settings.AppCulture = _culture;
        DialogResult = DialogResult.OK;
    }

    private void Cancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
    }

    private void CurrentCulture_CheckedChanged(object sender, EventArgs e)
    {
        if (radCurrentCulture.Checked)
        {
            _culture = System.Globalization.CultureInfo.CurrentCulture;
            UpdateUI_Language();
        }
    }

    private void InvariantCulture_CheckedChanged(object sender, EventArgs e)
    {
        if (radInvariantCulture.Checked)
        {
            _culture = System.Globalization.CultureInfo.InvariantCulture;
            UpdateUI_Language();
        }
    }

    private void UserCulture_CheckedChanged(object sender, EventArgs e)
    {
        cboAllCultures.Enabled = radUserCulture.Checked;
        if (cboAllCultures.Enabled)
        {
            _culture = new((string)cboAllCultures.SelectedValue ?? String.Empty);
            UpdateUI_Language();
        }
    }

    private void AllCultures_SelectedValueChanged(object sender, EventArgs e)
    {
        cboAllCultures.Enabled = radUserCulture.Checked;
        if (cboAllCultures.Enabled)
        {
            _culture = new((string)cboAllCultures.SelectedValue ?? String.Empty);
            UpdateUI_Language();
        }
    }

    /// <summary>
    /// Updates the form's controls with values from the settings class
    /// </summary>
    private void UpdateControls(string cultureName = "")
    {
        cboAllCultures.Enabled = false;
        if (cultureName == string.Empty)
            radInvariantCulture.Checked = true;
        else if (cultureName == System.Globalization.CultureInfo.CurrentCulture.Name)
            radCurrentCulture.Checked = true;
        else
        {
            cboAllCultures.SelectedValue = cultureName;
            radUserCulture.Checked = true;
        }
    }

    /// <summary>
    /// Databind only the cultures found in .resources files for a given type
    /// </summary>
    /// <param name="type">A type from which the resource manager derives all information for finding .resources files</param>
    private void FillDefinedCultures(string baseName, System.Reflection.Assembly assembly)
    {
        string cultureName = _culture.Name;
        var cultures = System.Globalization.GlobalizationUtilities.GetAvailableCultures(baseName, assembly);
        cboAllCultures.DisplayMember = "DisplayName";
        cboAllCultures.ValueMember = "Name";
        cboAllCultures.DataSource = cultures.ToArray();
        cboAllCultures.SelectedValue = cultureName;
    }

    /// <summary>
    /// Update the form's interface language
    /// </summary>
    /// <param name="culture">Culture used to display the UI</param>
    private void UpdateUI_Language()
    {
        UpdateUI_Language(_culture);
    }

    /// <summary>
    /// Update the form's interface language
    /// </summary>
    /// <param name="culture">Culture used to display the UI</param>
    private void UpdateUI_Language(System.Globalization.CultureInfo culture)
    {
        StringResources.Culture = culture;

        this.Text = StringResources.FrmLanguage;
        this.radCurrentCulture.Text = StringResources.RadCurrentCulture + $" ({System.Globalization.CultureInfo.CurrentCulture.Name})";
        this.radInvariantCulture.Text = StringResources.RadInvariantCulture;
        this.radUserCulture.Text = StringResources.RadUserCulture;
        this.btnCancel.Text = StringResources.BtnCancel;
        this.btnAccept.Text = StringResources.BtnAccept;
    }

}
