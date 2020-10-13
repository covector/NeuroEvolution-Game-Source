using UnityEngine.UI;
using UnityEngine;

public class ParameterManager : MonoBehaviour
{
    public Text hiddenplacehold;
    public Text hiddentext;
    public void UpdateHidden()
    {
        PlayerPrefs.SetInt("Hidden", int.Parse(hiddentext.text));
    }

    public Text inputtext;
    public Dropdown inputdropdown;
    public void UpdateInput()
    {
        PlayerPrefs.SetString("Input", inputtext.text);
    }

    public Toggle inverttoggle;
    public void UpdateInvert(bool toggle)
    {
        if (toggle) { PlayerPrefs.SetInt("Invert", 1); }
        else { PlayerPrefs.SetInt("Invert", 0); }
    }

    public Toggle timemasktoggle;
    public void UpdateTimeMask(bool toggle)
    {
        if (toggle) { PlayerPrefs.SetInt("TimeMask", 1); }
        else { PlayerPrefs.SetInt("TimeMask", 0); }
    }

    public Text outputtext;
    public Dropdown outputdropdown;
    public void UpdateOutput()
    {
        if (outputtext.text == "Bidirectional") { PlayerPrefs.SetString("Output", "Basisdirection"); }
        if (outputtext.text == "Multidirectional") { PlayerPrefs.SetString("Output", "Direction"); }
        if (outputtext.text == "Keystroke") { PlayerPrefs.SetString("Output", "Keystroke"); }
    }

    public Toggle centerdetectiontoggle;
    public void UpdateCenterDetection(bool toggle)
    {
        if (toggle) { PlayerPrefs.SetInt("CenterDetection", 1); }
        else { PlayerPrefs.SetInt("CenterDetection", 0); }
    }

    public Text raylengthplacehold;
    public Text raylengthtext;
    public void UpdateRayLength()
    {
        PlayerPrefs.SetFloat("RayLength", float.Parse(raylengthtext.text));
    }

    public Text angleplacehold;
    public Text angletext;
    public void UpdateAngle()
    {
        PlayerPrefs.SetFloat("Angle", float.Parse(angletext.text));
    }

    public Text offsetplacehold;
    public Text offsettext;
    public void UpdateOffset()
    {
        PlayerPrefs.SetFloat("Offset", float.Parse(offsettext.text));
    }

    public Text populationplacehold;
    public Text populationtext;
    public void UpdatePopulation()
    {
        PlayerPrefs.SetInt("Population", int.Parse(populationtext.text));
    }

    public Text mutationrateplacehold;
    public Text mutationratetext;
    public void UpdateMutationRate()
    {
        PlayerPrefs.SetFloat("MutationRate", float.Parse(mutationratetext.text));
    }

    public Text mutationscaleplacehold;
    public Text mutationscaletext;
    public void UpdateMutationScale()
    {
        PlayerPrefs.SetFloat("MutationScale", float.Parse(mutationscaletext.text));
    }

    public Text bestnplacehold;
    public Text bestntext;
    public void UpdateBestN()
    {
        PlayerPrefs.SetInt("BestN", int.Parse(bestntext.text));
    }

    public Text selectionmodetext;
    public Dropdown selectionmodedropdown;
    public void UpdateSelectionMode()
    {
        PlayerPrefs.SetString("SelectionMode", selectionmodetext.text);
    }

    public Text healthplacehold;
    public Text healthtext;
    public void UpdateHealth()
    {
        PlayerPrefs.SetInt("Health", int.Parse(healthtext.text));
    }

    public Text afkplacehold;
    public Text afktext;
    public Toggle afktoggle;
    public void UpdateAFK()
    {
        PlayerPrefs.SetFloat("AFKval", float.Parse(afktext.text));
    }
    public void UpdateAFK(bool toggle)
    {
        if (toggle) { PlayerPrefs.SetInt("AFK", 1); }
        else { PlayerPrefs.SetInt("AFK", 0); }
    }

    public Text onedafkplacehold;
    public Text onedafktext;
    public Toggle onedafktoggle;
    public void Update1dAFK()
    {
        PlayerPrefs.SetFloat("1dAFKval", float.Parse(onedafktext.text));
    }
    public void Update1dAFK(bool toggle)
    {
        if (toggle) { PlayerPrefs.SetInt("1dAFK", 1); }
        else { PlayerPrefs.SetInt("1dAFK", 0); }
    }

    public Toggle cornertoggle;
    public void UpdateCorner(bool toggle)
    {
        if (toggle) { PlayerPrefs.SetInt("Corner", 1); }
        else { PlayerPrefs.SetInt("Corner", 0); }
    }

    public Toggle walltoggle;
    public void UpdateWall(bool toggle)
    {
        if (toggle) { PlayerPrefs.SetInt("Wall", 1); }
        else { PlayerPrefs.SetInt("Wall", 0); }
    }

    private void Start()
    {
        hiddenplacehold.text = PlayerPrefs.GetInt("Hidden", 10).ToString();
        if (PlayerPrefs.GetString("Input", "Vector") == "Vector") { inputdropdown.value = 0; } else { inputdropdown.value = 1; }
        if (PlayerPrefs.GetInt("Invert", 1) == 1) { inverttoggle.isOn = true; } else { inverttoggle.isOn = false; }
        if (PlayerPrefs.GetInt("TimeMask", 1) == 1) { timemasktoggle.isOn = true; } else { timemasktoggle.isOn = false; }
        if (PlayerPrefs.GetString("Output", "Keystroke") == "Basisdirection") { outputdropdown.value = 0; } else { if (PlayerPrefs.GetString("Output", "Keystroke") == "Direction") { outputdropdown.value = 1; } else { outputdropdown.value = 2; } }
        if (PlayerPrefs.GetInt("CenterDetection", 1) == 1) { centerdetectiontoggle.isOn = true; } else { centerdetectiontoggle.isOn = false; }
        raylengthplacehold.text = PlayerPrefs.GetFloat("RayLength", 3f).ToString();
        angleplacehold.text = PlayerPrefs.GetFloat("Angle", 30f).ToString();
        offsetplacehold.text = PlayerPrefs.GetFloat("Offset", 0f).ToString();
        populationplacehold.text = PlayerPrefs.GetInt("Population", 100).ToString();
        mutationrateplacehold.text = PlayerPrefs.GetFloat("MutationRate", 0.1f).ToString();
        mutationscaleplacehold.text = PlayerPrefs.GetFloat("MutationScale", 0.5f).ToString();
        bestnplacehold.text = PlayerPrefs.GetInt("BestN", 5).ToString();
        if (PlayerPrefs.GetString("SelectionMode", "Time") == "Time") { selectionmodedropdown.value = 0; } else { selectionmodedropdown.value = 1; }
        healthplacehold.text = PlayerPrefs.GetInt("Health", 10).ToString();
        if (PlayerPrefs.GetInt("AFK", 0) == 1) { afktoggle.isOn = true; } else { afktoggle.isOn = false; }
        afkplacehold.text = PlayerPrefs.GetFloat("AFKval", 5f).ToString();
        if (PlayerPrefs.GetInt("1dAFK", 0) == 1) { onedafktoggle.isOn = true; } else { onedafktoggle.isOn = false; }
        onedafkplacehold.text = PlayerPrefs.GetFloat("1dAFKval", 15f).ToString();
        if (PlayerPrefs.GetInt("Corner", 0) == 1) { cornertoggle.isOn = true; } else { cornertoggle.isOn = false; }
        if (PlayerPrefs.GetInt("Wall", 1) == 1) { walltoggle.isOn = true; } else { walltoggle.isOn = false; }
    }
}
