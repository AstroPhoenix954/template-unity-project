﻿using UnityEngine;
using UnityEngine.UI;

namespace OmiyaGames.Menu
{
    using Settings;
    using System;

    ///-----------------------------------------------------------------------
    /// <copyright file="OptionsListMenu.cs" company="Omiya Games">
    /// The MIT License (MIT)
    /// 
    /// Copyright (c) 2014-2018 Omiya Games
    /// 
    /// Permission is hereby granted, free of charge, to any person obtaining a copy
    /// of this software and associated documentation files (the "Software"), to deal
    /// in the Software without restriction, including without limitation the rights
    /// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    /// copies of the Software, and to permit persons to whom the Software is
    /// furnished to do so, subject to the following conditions:
    /// 
    /// The above copyright notice and this permission notice shall be included in
    /// all copies or substantial portions of the Software.
    /// 
    /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    /// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    /// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    /// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    /// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    /// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    /// THE SOFTWARE.
    /// </copyright>
    /// <author>Taro Omiya</author>
    /// <date>6/11/2018</date>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// Menu that provides a list of option categories.
    /// You can retrieve this menu from the singleton script,
    /// <code>MenuManager</code>.
    /// </summary>
    /// <seealso cref="MenuManager"/>
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    public class OptionsListMenu : IMenu
    {
        [Header("Options List")]
        [SerializeField]
        Button backButton;
        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("allButtons")]
        PlatformSpecificButton[] allOptionsButtons;

        GameObject cachedDefaultButton = null;

        #region Overridden Properties
        public override Type MenuType
        {
            get
            {
                return Type.ManagedMenu;
            }
        }

        public override GameObject DefaultUi
        {
            get
            {
                if(cachedDefaultButton == null)
                {
                    foreach(PlatformSpecificButton button in allOptionsButtons)
                    {
                        if(button.EnabledFor.IsThisBuildSupported() == true)
                        {
                            cachedDefaultButton = button.Component.gameObject;
                            break;
                        }
                    }
                }
                return cachedDefaultButton;
            }
        }
        #endregion

        protected override void OnSetup()
        {
            // Call base method
            base.OnSetup();

            // Setup every button
            foreach (PlatformSpecificButton button in allOptionsButtons)
            {
                button.Setup();
            }
        }

        #region UI Events
        public void OnAudioClicked()
        {
            // Make sure the button isn't locked yet
            if (IsListeningToEvents == true)
            {
                // Show the audio options
                Manager.Show<OptionsAudioMenu>();
            }
        }

        public void OnControlsClicked()
        {
            // Make sure the button isn't locked yet
            if (IsListeningToEvents == true)
            {
                // Show the controls options
                Manager.Show<OptionsControlsMenu>();
            }
        }

        public void OnGraphicsClicked()
        {
            // Make sure the button isn't locked yet
            if (IsListeningToEvents == true)
            {
                // Show the graphics options
                Manager.Show<OptionsGraphicsMenu>();
            }
        }

        public void OnAccessibilityClicked()
        {
            // Make sure the button isn't locked yet
            if (IsListeningToEvents == true)
            {
                // Show the accessibility options
                Manager.Show<OptionsAccessibilityMenu>();
            }
        }

        public void OnLanguageClicked()
        {
            // Make sure the button isn't locked yet
            if (IsListeningToEvents == true)
            {
                // Show the language options
                Manager.Show<OptionsLanguageMenu>();
            }
        }

        public void OnResetDataClicked()
        {
            // Make sure the button isn't locked yet
            if (IsListeningToEvents == true)
            {
                ConfirmationMenu menu = Manager.GetMenu<ConfirmationMenu>();
                if (menu != null)
                {
                    // Display confirmation dialog
                    menu.DefaultToYes = false;
                    menu.Show(CheckResetSavedDataConfirmation);
                }
            }
        }
        #endregion

        #region Helper Methods
        void CheckResetSavedDataConfirmation(IMenu source, VisibilityState from, VisibilityState to)
        {
            if ((source is ConfirmationMenu) && (((ConfirmationMenu)source).IsYesSelected == true))
            {
                // Clear settings
                Singleton.Get<GameSettings>().ClearSettings();

                // Update the level select menu, if one is available
                LevelSelectMenu levelSelect = Manager.GetMenu<LevelSelectMenu>();
                if (levelSelect != null)
                {
                    levelSelect.SetButtonsEnabled(true);
                }
            }
        }
        #endregion
    }
}