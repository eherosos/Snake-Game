using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Elven_Path
{
    public class UIMain : MonoBehaviour
    {
        public void StartGame() { SceneManager.LoadScene(1); }
        public void Quit() { Application.Quit(); }
    }
}