using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Configura a posição da camera do game
/// </summary>
public class ArredondaPosCamera : CinemachineExtension {
    public float PixelsPerUnit = 32; // Quantidade de pixels dos objetos do game

    /*
	    Coloca a camera na melhor posição aproximada pelos pixels dos objetos
	*/
    protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage, ref CameraState state,
            float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            Vector3 pos = state.FinalPosition;
            Vector3 pos2 = new Vector3(Round(pos.x), Round(pos.y), Round(pos.z));
            state.PositionCorrection += pos2 - pos;
        }
    }

    /*
	    Arredonda a posição da camera de acordo com o pixels dos objetos do game
	*/
    float Round(float x)
    {
        return Mathf.Round(x * PixelsPerUnit) / PixelsPerUnit;
    }
}
