/// <summary>
/// Third Person Controller
/// <copyright>(c) AlexRak2TheDev or Alejandro Hernandez 2013</copyright>
/// 
/// Third Person Controller homepage: https://github.com/AlexRak2/PlayerControllerTemplates
/// 
/// This software is provided 'as-is', without any express or implied
/// warranty.  In no event will the authors be held liable for any damages
/// arising from the use of this software.
///
/// Permission is NOT granted to anyone to use this software for any purpose,
/// and to alter it and redistribute it freely.
///
/// 1. The origin of this software must not be misrepresented; you must not
/// claim that you wrote the original software. If you use this software
/// in a product, an acknowledgment in the product documentation would be
/// appreciated but is not required.
/// 2. Altered source versions must be plainly marked as such, and must not be
/// misrepresented as being the original software.
/// 3. This notice may not be removed or altered from any source distribution.
/// </summary>
/// 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsHide : MonoBehaviour
{
    public GameObject controlObj;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            controlObj.SetActive(!controlObj.activeSelf);
        }
    }
}
