/*
 * Created in SharpDevelop
 * Author: Kyle Fiegener
 * Date: 10/21/2010
 * Time: 4:39 AM
 */
using System;
// using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Media;
// using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.Win32;

namespace PsiComponents
{
    namespace Controls
    {
        /// <summary>
        /// Popup menu that properly makes Menu Popup and Menu Command sounds.
        /// Also provides additional graphical options for checkable menu items.
        /// </summary>
        public partial class PsiMenuItem /*extends*/: System.Windows.Forms.ToolStripMenuItem
        {
            protected static SoundPlayer PopupSounder;
            protected static SoundPlayer CommandSounder;
            const string PopupSoundRegPath = "AppEvents\\Schemes\\Apps\\.Default\\MenuPopup\\.Current";
            const string CommandSoundRegPath = "AppEvents\\Schemes\\Apps\\.Default\\MenuCommand\\.Current";
            
            static PsiMenuItem()
            {   // Read from the registry the file path to the system MenuPopup/Command sounds, and attach them to Popup/CommandSounder
                try
                {
                    PopupSounder = new SoundPlayer((String)Registry.CurrentUser.OpenSubKey(PopupSoundRegPath).GetValue(""));
                    PopupSounder.LoadAsync(); // preload sound now
                    PopupSounder.LoadCompleted += new AsyncCompletedEventHandler(Sounder_LoadCompleted); // register callback
                } catch (Exception) {} // Something gone wrong; don't actually care
                try
                {
                    CommandSounder = new SoundPlayer((String)Registry.CurrentUser.OpenSubKey(CommandSoundRegPath).GetValue(""));
                    CommandSounder.LoadAsync(); // preload sound now
                    CommandSounder.LoadCompleted += new AsyncCompletedEventHandler(Sounder_LoadCompleted); // register callback
                } catch (Exception) {} // Something gone wrong; don't actually care
            }
            
            // == "Sound file finished loading" event handler; plays loaded sound
            protected static void Sounder_LoadCompleted(object sender, AsyncCompletedEventArgs e)
            {
                SoundPlayer Sounder = (SoundPlayer)sender;
                
                if (Sounder.IsLoadCompleted)
                {
                    try
                    {
                        Sounder.Play();
                    } catch (Exception) {} // Something gone wrong; don't actually care
                }
            }
            
            
            private bool isCommandWithDropItems = false;
            /// <summary>
            /// Gets or sets whether this item executes a command when clicked even if it has drop items.
            /// </summary>
            public virtual bool IsCommandWithDropItems
            {
                get
                {
                    return isCommandWithDropItems;
                }
                set
                {
                    if (this.isCommandWithDropItems != value)
                        this.isCommandWithDropItems = value;
                }
            }
            
            private bool menuDoesNotClose = false;
            /// <summary>
            /// Gets or sets whether the menu stays open after this item is frobbed (could be good for checkable items).
            /// <em>Does not actually affect whether or not the menu closes yet.</em>
            /// </summary>
            public bool MenuDoesNotClose
            {   /* TODO: implement MenuDoesNotClose property actually affecting whether or not the menu closes!
                 * Should probably have visual cue for such items; at the very least item wouldn't highlight (much?) when hovered
                 * over. (The same cue would make sense for ToolStripLabel, too...)
                 */
                get
                {
                    return (this.menuDoesNotClose || (HasDropDownItems && !IsCommandWithDropItems));
                }
                set { menuDoesNotClose = value; }
            }

            /// <summary>
            /// CheckRenderMode property enum type
            /// </summary>
            public enum CheckRender
            {
                /// <summary>Standard checkmark-or-nothing behavior</summary>
                Default,
                /// <summary>Use System.Windows.Forms.CheckBox graphics.</summary>
                CheckBox,
                /// <summary>Use the custom image specified in the CustomCheckedImage property.</summary>
                Custom
            };
            private CheckRender checkRenderMode;
            public CheckRender CheckRenderMode
            {
                get { return checkRenderMode; }
                set { checkRenderMode = value; }
            }
            
            private Image customCheckedImage;
            /// <summary>
            /// Gets or sets the custom image to be used for showing a checkmark. Setting will change CheckRenderMode to Custom,
            /// unless setting to null; in which case CheckRenderMode will be set to Default if it is currently Custom.
            /// </summary>
            public Image CustomCheckedImage
            {
                get { return customCheckedImage; }
                set
                {
                    customCheckedImage = value;
                    
                    if (value != null)
                        CheckRenderMode = CheckRender.Custom;
                    else if (CheckRenderMode == CheckRender.Custom)
                        CheckRenderMode = CheckRender.Default;
                }
            }
                    
            /// <summary>
            /// Overriden to initiate playing of MenuPopup sound
            /// </summary>
            protected override void OnDropDownOpened(EventArgs e)
            {   base.OnDropDownOpened(e);
                if (PopupSounder.SoundLocation != null) // if popup sound set
                    PopupSounder.LoadAsync(); // load+play sound
            }
            
            /// <summary>
            /// Overriden to initiate playing of MenuCommand sound
            /// </summary>
            protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
            {   // TODO: make menu go away when IsCommandWithDropItems
                // TODO: make menu stay when MenuDoesNotClose
                base.OnMouseUp(e);
                
                // if clicking this item closes the menu, load+play menu command sound (if set)
                if (CommandSounder.SoundLocation != null && !this.MenuDoesNotClose && (!this.HasDropDownItems || this.IsCommandWithDropItems))
                    CommandSounder.LoadAsync(); // load+play sound
            }
            
            /* Override the OnPaint method to support replacing the default checkmark with other options. */
            protected override void OnPaint(PaintEventArgs e)
            {
                // If check mark is set to something besides default, temporarily clear the CheckState property
                // before calling base OnPaint to prevent the default check mark from being painted.
                if (this.CheckOnClick && this.CheckRenderMode != CheckRender.Default)
                {
                    CheckState currentState = this.CheckState;
                    Image currentImage = this.Image;
                                    
                    if (this.CheckRenderMode == CheckRender.Custom && this.Checked)
                    {
                        if (this.CustomCheckedImage == null)
                            throw new NullReferenceException("CheckRenderMode is Custom but CustomCheckedImage is null.");
                        else  // temporarily hijack the Image property to render custom check mark image
                            this.Image = CustomCheckedImage;
                    }
                    else if (this.CheckRenderMode == CheckRender.CheckBox)
                        this.CheckState = CheckState.Unchecked; // CheckState property temporarily cleared to prevent painting of default mark

                    // Do standard painting
                    base.OnPaint(e);
                    
                    // restore property values
                    this.CheckState = currentState;
                    this.Image = currentImage;
                }
                else // all normal; nothing to see here
                {
                    base.OnPaint(e);
                    return;
                }
                
                // Continue with any non-default check mark behavior
                
                // paint CheckBox control graphics in menu margin
                if (this.CheckRenderMode == CheckRender.CheckBox)
                {
                    // Determine the correct state of the CheckBox.
                    CheckBoxState boxState = CheckBoxState.UncheckedNormal; // initial value
                    if (this.Enabled)
                    {
                        if (this.Pressed)
                        {
                            if (this.Checked)  boxState = CheckBoxState.CheckedPressed;
                            else               boxState = CheckBoxState.UncheckedPressed;
                        }
                        else if (this.Selected)
                        {
                            if (this.Checked)  boxState = CheckBoxState.CheckedHot;
                            else               boxState = CheckBoxState.UncheckedHot;
                        }
                        else
                            if (this.Checked)  boxState = CheckBoxState.CheckedNormal;
                    }
                    else
                    {
                        if (this.Checked)  boxState = CheckBoxState.CheckedDisabled;
                        else               boxState = CheckBoxState.UncheckedDisabled;
                    }
                    
                    // Calculate the position at which to display the CheckBox.
                    int offsetY = (this.ContentRectangle.Height - CheckBoxRenderer.GetGlyphSize(e.Graphics, boxState).Height) / 2;
                    Point imageLocation = new Point(this.ContentRectangle.Location.X + 4, this.ContentRectangle.Location.Y + offsetY);
                
                    // Paint the CheckBox.
                    CheckBoxRenderer.DrawCheckBox(e.Graphics, imageLocation, boxState);
                }
            }
        }
    }
}
