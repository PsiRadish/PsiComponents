/*
 * Created in SharpDevelop
 * Author: Kyle Fiegener
 * Date: 10/23/2010
 * Time: 7:30 PM
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Resources;
// using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

// TODO: indexer
// TODO: += operator

namespace PsiComponents.Controls
{
    public class ItemNotInGroupException /*extends*/: Exception
    {
        //private static readonly string defaultMessage = "The ToolStripMenuItem is not a member of the RadioMenuGroup.";
        
        public ItemNotInGroupException()
            : base(/*defaultMessage*/) {}
    
        public ItemNotInGroupException(string message)
            : base(message) {}
    
        public ItemNotInGroupException(string message, Exception inner)
            : base(message, inner){}
    }
    
    /// <summary>
    /// Makes a number of ToolStripMenuItems into a "only one can be selected at a time" group. Items in the group will
    /// display radio buttons in their image/checkmark margin, unless the item has its Image property set (so don't go setting
    /// it). Don't go setting any CheckOnClick, Clicked, or ClickedState properties, either – they will be overriden/ignored.
    /// </summary>
    public class RadioMenuGroup /*implements*/: IEnumerable<ToolStripMenuItem>
    {
        protected static ResourceManager resources;
        
        static RadioMenuGroup()
        {
            resources = new ResourceManager(typeof(RadioMenuGroup).ToString(), Assembly.GetExecutingAssembly());
        }
        
        protected string name = null;
        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        /// <returns>A string representing the name. The default value is null.</returns>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        
        protected List<ToolStripMenuItem> items;
        /// <summary>
        /// Gets member ToolStripMenuItem at the specified index.
        /// </summary>
        public ToolStripMenuItem this[int i]
        {
            get { return this.items[i]; }
        }

        /// <summary>
        /// Gets the number of ToolStripMenuItems actually contained in the RadioMenuGroup.
        /// </summary>
        /// <returns>The number of ToolStripMenuItems actually contained in the RadioMenuGroup.</returns>
        public int Count
        {
            get { return this.items.Count; }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the RadioMenuGroup.
        /// </summary>
        /// <returns>Enumerator for the RadioMenuGroup.</returns>
        public IEnumerator<ToolStripMenuItem> GetEnumerator()
        {
            return ((IEnumerable<ToolStripMenuItem>)this.items).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the RadioMenuGroup.
        /// </summary>
        /// <returns>Enumerator for the RadioMenuGroup.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<ToolStripMenuItem>)this.items).GetEnumerator();
        }

        protected ToolStripMenuItem selectedItem = null;
        /// <summary>
        /// The currently selected item of the group. Throws ItemNotInGroupException when assigned an item that is not already in the group.
        /// </summary>
        public virtual ToolStripMenuItem SelectedItem
        {
            get { return selectedItem; }
            
            set
            {
                if (value != this.selectedItem) // if already selected, nothing to do
                {
                    if (items.Contains(value))
                        this.selectedItem = value;
                    else
                    {   // check if relevant objects have names assigned
                        string thisName  = this.Name;
                        thisName  = (thisName  != null) ? (thisName  + ' ') : ""; // add a space if something to show
                        
                        string alienName = (value as ToolStripMenuItem).Name;
                        alienName = (alienName != null) ? (alienName + ' ') : ""; // add a space if something to show
                        
                        throw new ItemNotInGroupException("The ToolStripMenuItem " + alienName + "is not a member of the " + thisName + "RadioMenuGroup.");
                    }
                }
            }
        }
        
        /// <summary>
        /// Create a new RadioMenuGroup without any members.
        /// </summary>
        public RadioMenuGroup()
        {
            this.items = new List<ToolStripMenuItem>();
        }

        /// <summary>
        /// Create a new RadioMenuGroup with the given array of member items.
        /// </summary>
        /// <param name="menuitems">Array of ToolStripMenuItems to be made members of the group.</param>
        public RadioMenuGroup(ToolStripMenuItem[] menuitems)
        {
            this.items = new List<ToolStripMenuItem>(menuitems.Length);
            this.AddRange(menuitems);
        }

        /// <summary>
        /// Create a new RadioMenuGroup with the given array of member items and a menu item to be initially selected.
        /// </summary>
        /// <param name="menuitems">Array of ToolStripMenuItems to be made members of the group.</param>
        /// <param name="initialSelected">The initially-selected ToolStripMenuItem of the group.</param>
        public RadioMenuGroup(ToolStripMenuItem[] menuitems, ToolStripMenuItem initialSelected)
            : this(menuitems)
        {
            this.SelectedItem = initialSelected;
        }
        
        /// <summary>
        /// Adds a single menu item to the group.
        /// </summary>
        /// <param name="item">ToolStripMenuItem to add.</param>
        public virtual void Add(ToolStripMenuItem item)
        {
            this.items.Add(item);
            
            // Add the gauntlet of event handlers that makes this all work
            item.Click          += new EventHandler(this.item_Clicked);
            item.CheckedChanged += new EventHandler(this.item_CheckedChanged);
            item.MouseEnter     += new EventHandler(this.item_MouseChange);
            item.MouseDown      += new MouseEventHandler(this.item_MouseChange);
            item.Paint          += new PaintEventHandler(this.item_Paint);
        }

        /// <summary>
        /// Adds an array of menu items to the group.
        /// </summary>
        /// <param name="menuitems">Array of ToolStripMenuItems to add.</param>
        public virtual void AddRange(ToolStripMenuItem[] menuitems)
        {
            foreach (ToolStripMenuItem item in menuitems)
                this.Add(item);
        }
        
        // TODO: RadioMenuGroup.Remove and RadioMenuGroup.RemoveRange

        // ==EVENT HANDLERS================================================ //
        
        protected virtual void item_Clicked(object sender, EventArgs e)
        {
            this.SelectedItem = (sender as ToolStripMenuItem);
        }
        
        protected virtual void item_CheckedChanged(object sender, EventArgs e)
        {
            (sender as ToolStripMenuItem).CheckOnClick = false; // No! No check marks!
            (sender as ToolStripMenuItem).Checked = false;
        }
        
        protected virtual void item_MouseChange(object sender, EventArgs e)
        {
            (sender as ToolStripItem).Invalidate(); // Verily, to the sender we say: redraw thyself.
        }
        
        // Paint the RadioButton where the check mark is normally displayed.
        protected virtual void item_Paint(object sender, PaintEventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            bool bulleted = (item == this.SelectedItem);
            
            if (item.Image != null)
            {   // If the client sets the Image property, the selection behavior remains unchanged, but the RadioButton is not
                // displayed and the selection is indicated only by the selection rectangle.
                return;
            }
            
            // Determine the correct state of the RadioButton.
            RadioButtonState buttonState = RadioButtonState.UncheckedNormal; // initial value
            
            if (item.Enabled)
            {
                if (item.Pressed)
                {
                    if (bulleted)  buttonState = RadioButtonState.CheckedPressed;
                    else           buttonState = RadioButtonState.UncheckedPressed;
                }
                else if (item.Selected)
                {
                    if (bulleted)  buttonState = RadioButtonState.CheckedHot;
                    else           buttonState = RadioButtonState.UncheckedHot;
                }
                else
                    if (bulleted)  buttonState = RadioButtonState.CheckedNormal;
            }
            else
            {
                if (bulleted)  buttonState = RadioButtonState.CheckedDisabled;
                else           buttonState = RadioButtonState.UncheckedDisabled;
            }
            
            // Calculate the position at which to display the RadioButton.
            int offset = (item.ContentRectangle.Height - RadioButtonRenderer.GetGlyphSize(e.Graphics, buttonState).Height) / 2;
            Point imageLocation = new Point(item.ContentRectangle.Location.X + 4, item.ContentRectangle.Location.Y + offset);
            
            // Paint the RadioButton.
            RadioButtonRenderer.DrawRadioButton(e.Graphics, imageLocation, buttonState);
        }
    }
}
