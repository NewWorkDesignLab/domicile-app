using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorSimulator : Singleton<EditorSimulator> {

    [Header ("WebGL URL Parameter")]
    [TextArea]
    public string URLParameter = "In WebGL Export, the User Authentication and Scenario Selection will be passed via URL Parameters. In Editor, the Parameters cant be fetched, so User Cretentials and ScenarioID defined below are used.";
    public string debugWebglUrlParameterEmail;
    public string debugWebglUrlParameterPassword;
    public int debugWebglUrlParameterScenario;
}
