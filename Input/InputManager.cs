using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SmallGalaxy_Engine
{
    public class InputManager
    {

        #region Action Enumeration

        /// <summary>
        /// The actions that are possible within the game.
        /// </summary>
        public enum Action
        {
            StartMenu,
            BackMenu,
            AltMenu,
            Ok,
            Back,            
            CursorUp,
            CursorDown,
            DecreaseAmount,
            IncreaseAmount,
            PageLeft,
            PageRight,
            LeftThumbStick,
            RightThumbStick,
            Dpad,
            WASDKeys,
            ArrowKeys,
            TotalActionCount,
        }

        /// <summary>
        /// Readable names of each action.
        /// </summary>
        private static readonly string[] actionNames = 
            {
                "Start Menu",
                "Back Menu",  
                "Alternate Menu",
                "Ok",
                "Back / Cancel",
                "Move Cursor - Up",
                "Move Cursor - Down",
                "Decrease Amount",
                "Increase Amount",
                "Page Screen Left",
                "Page Screen Right",
                "Left ThumbStick",
                "Right ThumbStick",
                "Dpad",
                "WASD Keys",
                "Arrow Keys",
            };

        /// <summary>
        /// Returns the readable name of the given action.
        /// </summary>
        public static string GetActionName(Action action)
        {
            int index = (int)action;

            if ((index < 0) || (index > actionNames.Length))
            {
                throw new ArgumentException("action");
            }

            return actionNames[index];
        }

        #endregion // Action Enumeration


        #region ActionMap Class

        /// <summary>
        /// A combination of gamepad and keyboard keys mapped to a particular action.
        /// </summary>
        public class ActionMap
        {
            /// <summary>
            /// List of GamePad controls to be mapped to a given action.
            /// </summary>
            public List<Buttons> gamePadButtons = new List<Buttons>();


            /// <summary>
            /// List of Keyboard controls to be mapped to a given action.
            /// </summary>
            public List<Keys> keyboardKeys = new List<Keys>();
        }

        #endregion // ActionMap Class


        #region Constants

        /// <summary>
        /// The value of an analog control that reads as a "pressed button".
        /// </summary>
        public const float analogLimit = 0.5f;

        /// <summary>
        /// The Maximum number of input devices
        /// </summary>
        public const int MaxInputs = 4;

        #endregion


        #region Properties

        public static PlayerIndex? ControllingPlayerIndex
        {
            get { return controllingPlayerIndex; }
            set { if (value.HasValue) { controllingPlayerIndex = value.Value; } }
        }
        private static PlayerIndex controllingPlayerIndex;

        public static KeyboardState[] CurrentKeyboardStates = new KeyboardState[MaxInputs];
        public static GamePadState[] CurrentGamePadStates = new GamePadState[MaxInputs];
        public static MouseState CurrentMouseState;

        public static KeyboardState[] PreviousKeyboardStates = new KeyboardState[MaxInputs];
        public static GamePadState[] PreviousGamePadStates = new GamePadState[MaxInputs];
        public static MouseState PreviouseMouseState;

        #region Buttons Array
        private static Buttons[] buttons = 
        {
            Buttons.Start,
            Buttons.Back,

            Buttons.A,
            Buttons.B,
            Buttons.X,
            Buttons.Y,

            Buttons.LeftShoulder,
            Buttons.RightShoulder,

            Buttons.LeftTrigger,
            Buttons.RightTrigger,

            Buttons.DPadUp,
            Buttons.DPadDown,
            Buttons.DPadLeft,
            Buttons.DPadRight,

            Buttons.LeftThumbstickUp,
            Buttons.LeftThumbstickDown,
            Buttons.LeftThumbstickLeft,
            Buttons.LeftThumbstickRight,

            Buttons.RightThumbstickUp,
            Buttons.RightThumbstickDown,
            Buttons.RightThumbstickLeft,
            Buttons.RightThumbstickRight,

            Buttons.LeftStick,
            Buttons.RightStick,            
        };
        #endregion // Butons Array

        public static Dictionary<Buttons, float>[] HeldGamePadButtons =
            new Dictionary<Buttons, float>[MaxInputs];
        public static float[] HeldGamePadButtonsTime = new float[MaxInputs];

        public static readonly bool[] GamePadWasConnected =
            new bool[MaxInputs];

        #endregion // Properties


        #region Mouse Data

        public static int MouseScroll()
        {
            return CurrentMouseState.ScrollWheelValue - PreviouseMouseState.ScrollWheelValue;
        }
        public static bool MouseHasMoved()
        {
            return (CurrentMouseState.X != PreviouseMouseState.X ||
                CurrentMouseState.Y != PreviouseMouseState.Y);
        }

        #region Triggered

        public static bool IsMouseLeftTriggered()
        {
            return (CurrentMouseState.LeftButton == ButtonState.Pressed &&
                    PreviouseMouseState.LeftButton != ButtonState.Pressed);
        }

        public static bool IsMouseRightTriggered()
        {
            return (CurrentMouseState.RightButton == ButtonState.Pressed &&
                        PreviouseMouseState.RightButton != ButtonState.Pressed);
        }

        #endregion // Triggered


        #region Pressed

        public static bool IsMouseLeftPressed()
        {
            return (CurrentMouseState.LeftButton == ButtonState.Pressed);
        }

        public static bool IsMouseRightPressed()
        {
            return (CurrentMouseState.RightButton == ButtonState.Pressed);
        }

        #endregion // Pressed


        #region Released

        public static bool IsMouseLeftReleased()
        {
            return (CurrentMouseState.LeftButton != ButtonState.Pressed &&
                    PreviouseMouseState.LeftButton == ButtonState.Pressed);
        }

        public static bool IsMouseRightReleased()
        {
            return (CurrentMouseState.RightButton != ButtonState.Pressed &&
                        PreviouseMouseState.RightButton == ButtonState.Pressed);
        }

        #endregion // Released


        #endregion // Mouse Data


        #region Keyboard Data


        #region Triggered

        public static bool IsKeyTriggered(Keys key)
        {
            return (IsKeyTriggered(key, ControllingPlayerIndex, out controllingPlayerIndex));
        }

        /// <summary>
        /// Check if a key was just pressed in the most recent update.
        /// </summary>
        public static bool IsKeyTriggered(Keys key, PlayerIndex? controllingPlayer,
            out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;
                return (CurrentKeyboardStates[i].IsKeyDown(key)) &&
                    (PreviousKeyboardStates[i].IsKeyUp(key));
            }
            else
            {
                return (IsKeyTriggered(key, PlayerIndex.One, out playerIndex) ||
                        IsKeyTriggered(key, PlayerIndex.Two, out playerIndex) ||
                        IsKeyTriggered(key, PlayerIndex.Three, out playerIndex) ||
                        IsKeyTriggered(key, PlayerIndex.Four, out playerIndex));
            }


        }

        #endregion // Triggered


        #region Pressed

        public static bool IsKeyPressed(Keys key)
        {
            return (IsKeyPressed(key, ControllingPlayerIndex, out controllingPlayerIndex));
        }

        /// <summary>
        /// Check if a key is pressed.
        /// </summary>
        public static bool IsKeyPressed(Keys key, PlayerIndex? controllingPlayer,
            out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;
                return CurrentKeyboardStates[i].IsKeyDown(key);
            }
            else
            {
                return (IsKeyPressed(key, PlayerIndex.One, out playerIndex) ||
                        IsKeyPressed(key, PlayerIndex.Two, out playerIndex) ||
                        IsKeyPressed(key, PlayerIndex.Three, out playerIndex) ||
                        IsKeyPressed(key, PlayerIndex.Four, out playerIndex));
            }
        }

        #endregion // Pressed


        #region Held

        #endregion // Held


        #endregion // Keyboard Data


        #region GamePad Data

        /// <summary>
        /// The state of the gamepad as of the last update.
        /// </summary>
        public static GamePadState CurrentGamePadState
        {
            get { return CurrentGamePadStates[(int)ControllingPlayerIndex]; }
        }

        public static GamePadState PlayerCurrentGamePadState(PlayerIndex controllingPlayer)
        {
            return CurrentGamePadStates[(int)controllingPlayer];
        }

        public static Vector2 LeftThumbStick
        {
            get { return CurrentGamePadState.ThumbSticks.Left; }
        }

        public static Vector2 RightThumbStick
        {
            get { return CurrentGamePadState.ThumbSticks.Right; }
        }

        /// <summary>
        /// Left Trigger float
        /// </summary>
        public static float LeftTrigger
        {
            get { return CurrentGamePadState.Triggers.Left; }
        }

        /// <summary>
        /// Right Trigger float
        /// </summary>
        public static float RightTrigger
        {
            get { return CurrentGamePadState.Triggers.Right; }
        }


        #region Pressed

        public static bool IsGamePadLeftThumbStickPressed
        {
            get { return (GamePadLeftThumbStickPressed(ControllingPlayerIndex, out controllingPlayerIndex)); }
        }

        /// <summary>
        /// Check if the GamePad Left ThumbStick is pressed
        /// </summary>
        public static bool GamePadLeftThumbStickPressed(PlayerIndex? controllingPlayer,
            out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;
                int i = (int)playerIndex;

                return (Math.Abs(CurrentGamePadStates[i].ThumbSticks.Left.X) > analogLimit ||
                        Math.Abs(CurrentGamePadStates[i].ThumbSticks.Left.Y) > analogLimit);
            }
            else
            {
                return (GamePadLeftThumbStickPressed(PlayerIndex.One, out playerIndex) ||
                        GamePadLeftThumbStickPressed(PlayerIndex.Two, out playerIndex) ||
                        GamePadLeftThumbStickPressed(PlayerIndex.Three, out playerIndex) ||
                        GamePadLeftThumbStickPressed(PlayerIndex.Four, out playerIndex));
            }
        }

        public static bool IsGamePadRightThumbStickPressed
        {
            get { return (GamePadRightThumbStickPressed(ControllingPlayerIndex, out controllingPlayerIndex)); }
        }

        /// <summary>
        /// Check if the GamePad Right ThumbStick is pressed
        /// </summary>
        public static bool GamePadRightThumbStickPressed(PlayerIndex? controllingPlayer,
            out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;
                int i = (int)playerIndex;

                return (Math.Abs(CurrentGamePadStates[i].ThumbSticks.Right.X) > analogLimit ||
                        Math.Abs(CurrentGamePadStates[i].ThumbSticks.Right.Y) > analogLimit);
            }
            else
            {
                return (GamePadRightThumbStickPressed(PlayerIndex.One, out playerIndex) ||
                        GamePadRightThumbStickPressed(PlayerIndex.Two, out playerIndex) ||
                        GamePadRightThumbStickPressed(PlayerIndex.Three, out playerIndex) ||
                        GamePadRightThumbStickPressed(PlayerIndex.Four, out playerIndex));
            }
        }

        public static bool IsGamePadButtonPressed(Buttons gamePadKey)
        {
            return IsGamePadButtonPressed(gamePadKey, ControllingPlayerIndex, out controllingPlayerIndex);
        }

        /// <summary>
        /// Check if the GamePadKey value specified is pressed.
        /// </summary>
        public static bool IsGamePadButtonPressed(Buttons gamePadKey, PlayerIndex? controllingPlayer,
            out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;
                int i = (int)playerIndex;

                if (gamePadKey == Buttons.LeftTrigger ||
                    gamePadKey == Buttons.RightTrigger ||

                    gamePadKey == Buttons.LeftThumbstickDown ||
                    gamePadKey == Buttons.LeftThumbstickLeft ||
                    gamePadKey == Buttons.LeftThumbstickRight ||
                    gamePadKey == Buttons.LeftThumbstickUp ||

                    gamePadKey == Buttons.RightThumbstickDown ||
                    gamePadKey == Buttons.RightThumbstickLeft ||
                    gamePadKey == Buttons.RightThumbstickRight ||
                    gamePadKey == Buttons.RightThumbstickUp)
                {
                    switch (gamePadKey)
                    {
                        case Buttons.LeftTrigger:
                            return CurrentGamePadStates[i].Triggers.Left > analogLimit;
                        case Buttons.RightTrigger:
                            return CurrentGamePadStates[i].Triggers.Right > analogLimit;

                        case Buttons.LeftThumbstickUp:
                            return CurrentGamePadStates[i].ThumbSticks.Left.Y > analogLimit;
                        case Buttons.LeftThumbstickDown:
                            return CurrentGamePadStates[i].ThumbSticks.Left.Y < -analogLimit;
                        case Buttons.LeftThumbstickLeft:
                            return CurrentGamePadStates[i].ThumbSticks.Left.X < -analogLimit;
                        case Buttons.LeftThumbstickRight:
                            return CurrentGamePadStates[i].ThumbSticks.Left.X > analogLimit;

                        case Buttons.RightThumbstickUp:
                            return CurrentGamePadStates[i].ThumbSticks.Right.Y > analogLimit;
                        case Buttons.RightThumbstickDown:
                            return CurrentGamePadStates[i].ThumbSticks.Right.Y < -analogLimit;
                        case Buttons.RightThumbstickLeft:
                            return CurrentGamePadStates[i].ThumbSticks.Right.X < -analogLimit;
                        case Buttons.RightThumbstickRight:
                            return CurrentGamePadStates[i].ThumbSticks.Right.X > analogLimit;
                        default:
                            return false;
                    }
                }
                else
                {
                    return CurrentGamePadStates[i].IsButtonDown(gamePadKey);
                }
            }
            else
            {
                return (IsGamePadButtonPressed(gamePadKey, PlayerIndex.One, out playerIndex) ||
                        IsGamePadButtonPressed(gamePadKey, PlayerIndex.Two, out playerIndex) ||
                        IsGamePadButtonPressed(gamePadKey, PlayerIndex.Three, out playerIndex) ||
                        IsGamePadButtonPressed(gamePadKey, PlayerIndex.Four, out playerIndex));
            }
        }

        #endregion // Pressed


        #region Triggered

        public static bool IsGamePadLeftThumbStickTriggered
        {
            get { return (GamePadLeftThumbStickTriggered(ControllingPlayerIndex, out controllingPlayerIndex)); }
        }

        /// <summary>
        /// Check if the GamePad Left ThumbStick is pressed
        /// </summary>
        public static bool GamePadLeftThumbStickTriggered(PlayerIndex? controllingPlayer,
            out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;
                int i = (int)playerIndex;

                return ((Math.Abs(CurrentGamePadStates[i].ThumbSticks.Left.X) > analogLimit ||
                        Math.Abs(CurrentGamePadStates[i].ThumbSticks.Left.Y) > analogLimit) &&
                        (Math.Abs(PreviousGamePadStates[i].ThumbSticks.Left.X) <= analogLimit &&
                        Math.Abs(PreviousGamePadStates[i].ThumbSticks.Left.Y) <= analogLimit));
            }
            else
            {
                return (GamePadLeftThumbStickTriggered(PlayerIndex.One, out playerIndex) ||
                        GamePadLeftThumbStickTriggered(PlayerIndex.Two, out playerIndex) ||
                        GamePadLeftThumbStickTriggered(PlayerIndex.Three, out playerIndex) ||
                        GamePadLeftThumbStickTriggered(PlayerIndex.Four, out playerIndex));
            }
        }

        public static bool IsGamePadRightThumbStickTriggered
        {
            get { return (GamePadRightThumbStickTriggered(ControllingPlayerIndex, out controllingPlayerIndex)); }
        }

        /// <summary>
        /// Check if the GamePad Right ThumbStick is pressed
        /// </summary>
        public static bool GamePadRightThumbStickTriggered(PlayerIndex? controllingPlayer,
            out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;
                int i = (int)playerIndex;

                return ((Math.Abs(CurrentGamePadStates[i].ThumbSticks.Right.X) > analogLimit ||
                        Math.Abs(CurrentGamePadStates[i].ThumbSticks.Right.Y) > analogLimit) &&
                        (Math.Abs(PreviousGamePadStates[i].ThumbSticks.Right.X) <= analogLimit &&
                        Math.Abs(PreviousGamePadStates[i].ThumbSticks.Right.Y) <= analogLimit));
            }
            else
            {
                return (GamePadRightThumbStickTriggered(PlayerIndex.One, out playerIndex) ||
                        GamePadRightThumbStickTriggered(PlayerIndex.Two, out playerIndex) ||
                        GamePadRightThumbStickTriggered(PlayerIndex.Three, out playerIndex) ||
                        GamePadRightThumbStickTriggered(PlayerIndex.Four, out playerIndex));
            }
        }


        public static bool IsGamePadButtonTriggered(Buttons gamePadKey)
        {
            return (GamePadButtonTriggered(gamePadKey, ControllingPlayerIndex, out controllingPlayerIndex));
        }

        /// <summary>
        /// Check if the GamePadKey value specified was triggered this frame.
        /// </summary>
        public static bool GamePadButtonTriggered(Buttons gamePadKey, PlayerIndex? controllingPlayer,
            out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;
                int i = (int)playerIndex;

                if (gamePadKey == Buttons.LeftTrigger ||
                    gamePadKey == Buttons.RightTrigger ||

                    gamePadKey == Buttons.LeftThumbstickDown ||
                    gamePadKey == Buttons.LeftThumbstickLeft ||
                    gamePadKey == Buttons.LeftThumbstickRight ||
                    gamePadKey == Buttons.LeftThumbstickUp ||

                    gamePadKey == Buttons.RightThumbstickDown ||
                    gamePadKey == Buttons.RightThumbstickLeft ||
                    gamePadKey == Buttons.RightThumbstickRight ||
                    gamePadKey == Buttons.RightThumbstickUp)
                {
                    switch (gamePadKey)
                    {
                        case Buttons.LeftTrigger:
                            return (CurrentGamePadStates[i].Triggers.Left > analogLimit &&
                                    PreviousGamePadStates[i].Triggers.Left <= analogLimit);
                        case Buttons.RightTrigger:
                            return (CurrentGamePadStates[i].Triggers.Right > analogLimit &&
                                    PreviousGamePadStates[i].Triggers.Right <= analogLimit);

                        case Buttons.LeftThumbstickUp:
                            return (CurrentGamePadStates[i].ThumbSticks.Left.Y > analogLimit &&
                                    PreviousGamePadStates[i].ThumbSticks.Left.Y <= analogLimit);
                        case Buttons.LeftThumbstickDown:
                            return (CurrentGamePadStates[i].ThumbSticks.Left.Y < -analogLimit &&
                                    PreviousGamePadStates[i].ThumbSticks.Left.Y >= -analogLimit);
                        case Buttons.LeftThumbstickLeft:
                            return (CurrentGamePadStates[i].ThumbSticks.Left.X < -analogLimit &&
                                    PreviousGamePadStates[i].ThumbSticks.Left.X >= -analogLimit);
                        case Buttons.LeftThumbstickRight:
                            return (CurrentGamePadStates[i].ThumbSticks.Left.X > analogLimit &&
                                    PreviousGamePadStates[i].ThumbSticks.Left.X <= analogLimit);

                        case Buttons.RightThumbstickUp:
                            return (CurrentGamePadStates[i].ThumbSticks.Right.Y > analogLimit &&
                                    PreviousGamePadStates[i].ThumbSticks.Right.Y <= analogLimit);
                        case Buttons.RightThumbstickDown:
                            return (CurrentGamePadStates[i].ThumbSticks.Right.Y < -analogLimit &&
                                    PreviousGamePadStates[i].ThumbSticks.Right.Y >= -analogLimit);
                        case Buttons.RightThumbstickLeft:
                            return (CurrentGamePadStates[i].ThumbSticks.Right.X < -analogLimit &&
                                    PreviousGamePadStates[i].ThumbSticks.Right.X >= -analogLimit);
                        case Buttons.RightThumbstickRight:
                            return (CurrentGamePadStates[i].ThumbSticks.Right.X > analogLimit &&
                                    PreviousGamePadStates[i].ThumbSticks.Right.X <= analogLimit);
                        default:
                            return false;
                    }
                }
                else
                {
                    return (CurrentGamePadStates[i].IsButtonDown(gamePadKey) &&
                            PreviousGamePadStates[i].IsButtonUp(gamePadKey));
                }
            }
            else
            {
                return (GamePadButtonTriggered(gamePadKey, PlayerIndex.One, out playerIndex) ||
                        GamePadButtonTriggered(gamePadKey, PlayerIndex.Two, out playerIndex) ||
                        GamePadButtonTriggered(gamePadKey, PlayerIndex.Three, out playerIndex) ||
                        GamePadButtonTriggered(gamePadKey, PlayerIndex.Four, out playerIndex));
            }
        }

        #endregion // Triggered


        #region Released

        /// <summary>
        /// Check if the GamePad Left ThumbStick is Released
        /// </summary>
        public static bool IsGamePadLeftThumbStickReleased
        {
            get { return (GamePadLeftThumbStickReleased(ControllingPlayerIndex, out controllingPlayerIndex)); }
        }

        public static bool GamePadLeftThumbStickReleased(PlayerIndex? controllingPlayer,
            out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;
                int i = (int)playerIndex;

                return ((Math.Abs(CurrentGamePadStates[i].ThumbSticks.Left.X) <= analogLimit &&
                        Math.Abs(CurrentGamePadStates[i].ThumbSticks.Left.Y) <= analogLimit) &&
                        (Math.Abs(PreviousGamePadStates[i].ThumbSticks.Left.X) > analogLimit ||
                        Math.Abs(PreviousGamePadStates[i].ThumbSticks.Left.Y) > analogLimit));
            }
            else
            {
                return (GamePadLeftThumbStickReleased(PlayerIndex.One, out playerIndex) ||
                        GamePadLeftThumbStickReleased(PlayerIndex.Two, out playerIndex) ||
                        GamePadLeftThumbStickReleased(PlayerIndex.Three, out playerIndex) ||
                        GamePadLeftThumbStickReleased(PlayerIndex.Four, out playerIndex));
            }
        }


        /// <summary>
        /// Check if the GamePad Right ThumbStick is Released
        /// </summary>
        public static bool IsGamePadRightThumbStickReleased
        {
            get { return (GamePadRightThumbStickReleased(ControllingPlayerIndex, out controllingPlayerIndex)); }
        }

        public static bool GamePadRightThumbStickReleased(PlayerIndex? controllingPlayer,
            out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;
                int i = (int)playerIndex;

                return ((Math.Abs(CurrentGamePadStates[i].ThumbSticks.Right.X) <= analogLimit &&
                        Math.Abs(CurrentGamePadStates[i].ThumbSticks.Right.Y) <= analogLimit) &&
                        (Math.Abs(PreviousGamePadStates[i].ThumbSticks.Right.X) > analogLimit ||
                        Math.Abs(PreviousGamePadStates[i].ThumbSticks.Right.Y) > analogLimit));
            }
            else
            {
                return (GamePadRightThumbStickReleased(PlayerIndex.One, out playerIndex) ||
                        GamePadRightThumbStickReleased(PlayerIndex.Two, out playerIndex) ||
                        GamePadRightThumbStickReleased(PlayerIndex.Three, out playerIndex) ||
                        GamePadRightThumbStickReleased(PlayerIndex.Four, out playerIndex));
            }
        }


        public static bool IsGamePadButtonReleased(Buttons gamePadKey)
        {
            return (GamePadButtonReleased(gamePadKey, ControllingPlayerIndex, out controllingPlayerIndex));
        }

        /// <summary>
        /// Check if the GamePadKey value specified was Released this frame.
        /// </summary>
        public static bool GamePadButtonReleased(Buttons gamePadKey, PlayerIndex? controllingPlayer,
            out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;
                int i = (int)playerIndex;

                if (gamePadKey == Buttons.LeftTrigger ||
                    gamePadKey == Buttons.RightTrigger ||

                    gamePadKey == Buttons.LeftThumbstickDown ||
                    gamePadKey == Buttons.LeftThumbstickLeft ||
                    gamePadKey == Buttons.LeftThumbstickRight ||
                    gamePadKey == Buttons.LeftThumbstickUp ||

                    gamePadKey == Buttons.RightThumbstickDown ||
                    gamePadKey == Buttons.RightThumbstickLeft ||
                    gamePadKey == Buttons.RightThumbstickRight ||
                    gamePadKey == Buttons.RightThumbstickUp)
                {
                    switch (gamePadKey)
                    {
                        case Buttons.LeftTrigger:
                            return (CurrentGamePadStates[i].Triggers.Left <= analogLimit &&
                                    PreviousGamePadStates[i].Triggers.Left > analogLimit);
                        case Buttons.RightTrigger:
                            return (CurrentGamePadStates[i].Triggers.Right <= analogLimit &&
                                    PreviousGamePadStates[i].Triggers.Right > analogLimit);

                        case Buttons.LeftThumbstickUp:
                            return (CurrentGamePadStates[i].ThumbSticks.Left.Y <= analogLimit &&
                                    PreviousGamePadStates[i].ThumbSticks.Left.Y > analogLimit);
                        case Buttons.LeftThumbstickDown:
                            return (CurrentGamePadStates[i].ThumbSticks.Left.Y >= -analogLimit &&
                                    PreviousGamePadStates[i].ThumbSticks.Left.Y < -analogLimit);
                        case Buttons.LeftThumbstickLeft:
                            return (CurrentGamePadStates[i].ThumbSticks.Left.X >= -analogLimit &&
                                    PreviousGamePadStates[i].ThumbSticks.Left.X < -analogLimit);
                        case Buttons.LeftThumbstickRight:
                            return (CurrentGamePadStates[i].ThumbSticks.Left.X <= analogLimit &&
                                    PreviousGamePadStates[i].ThumbSticks.Left.X > analogLimit);

                        case Buttons.RightThumbstickUp:
                            return (CurrentGamePadStates[i].ThumbSticks.Right.Y <= analogLimit &&
                                    PreviousGamePadStates[i].ThumbSticks.Right.Y > analogLimit);
                        case Buttons.RightThumbstickDown:
                            return (CurrentGamePadStates[i].ThumbSticks.Right.Y >= -analogLimit &&
                                    PreviousGamePadStates[i].ThumbSticks.Right.Y < -analogLimit);
                        case Buttons.RightThumbstickLeft:
                            return (CurrentGamePadStates[i].ThumbSticks.Right.X >= -analogLimit &&
                                    PreviousGamePadStates[i].ThumbSticks.Right.X < -analogLimit);
                        case Buttons.RightThumbstickRight:
                            return (CurrentGamePadStates[i].ThumbSticks.Right.X <= analogLimit &&
                                    PreviousGamePadStates[i].ThumbSticks.Right.X > analogLimit);
                        default:
                            return false;
                    }
                }
                else
                {
                    return (CurrentGamePadStates[i].IsButtonUp(gamePadKey) &&
                            PreviousGamePadStates[i].IsButtonDown(gamePadKey));
                }
            }
            else
            {
                return (GamePadButtonTriggered(gamePadKey, PlayerIndex.One, out playerIndex) ||
                        GamePadButtonTriggered(gamePadKey, PlayerIndex.Two, out playerIndex) ||
                        GamePadButtonTriggered(gamePadKey, PlayerIndex.Three, out playerIndex) ||
                        GamePadButtonTriggered(gamePadKey, PlayerIndex.Four, out playerIndex));
            }
        }

        #endregion // Released


        #region Held

        public static bool IsGamePadButtonHeld(Buttons button, float time)
        {
            return GamePadButtonHeld(button, time, ControllingPlayerIndex, out controllingPlayerIndex);
        }

        public static bool GamePadButtonHeld(Buttons button, float time,
            PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;
                int i = (int)playerIndex;

                if (HeldGamePadButtons[i].ContainsKey(button))
                {
                    if (HeldGamePadButtonsTime[i] >= HeldGamePadButtons[i][button] + time)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return (GamePadButtonHeld(button, time, PlayerIndex.One, out playerIndex) ||
                        GamePadButtonHeld(button, time, PlayerIndex.Two, out playerIndex) ||
                        GamePadButtonHeld(button, time, PlayerIndex.Three, out playerIndex) ||
                        GamePadButtonHeld(button, time, PlayerIndex.Four, out playerIndex));
            }
        }

        public static void ResetHeldButtonTimer(Buttons button)
        {
            if (ControllingPlayerIndex.HasValue)
            {
                ResetHeldButtonTimer(button, ControllingPlayerIndex.Value);
            }
        }

        public static void ResetHeldButtonTimer(Buttons button, PlayerIndex controllingPlayer)
        {
            int i = (int)controllingPlayer;
            if (HeldGamePadButtons[i].ContainsKey(button))
            {
                HeldGamePadButtons[i][button] = HeldGamePadButtonsTime[i];
            }
        }

        #endregion // GamePadButtons Held Queries


        #endregion // GamePad Data


        #region Action Mapping

        /// <summary>
        /// The action mappings for the game.
        /// </summary>
        private static ActionMap[] actionMaps;

        public static ActionMap[] ActionMaps
        {
            get { return actionMaps; }
        }


        /// <summary>
        /// Reset the action maps to their default values.
        /// </summary>
        private static void ResetActionMaps()
        {
            actionMaps = new ActionMap[(int)Action.TotalActionCount];

            actionMaps[(int)Action.StartMenu] = new ActionMap();
            actionMaps[(int)Action.StartMenu].keyboardKeys.Add(
                Keys.Tab);
            actionMaps[(int)Action.StartMenu].gamePadButtons.Add(
                Buttons.Start);

            actionMaps[(int)Action.BackMenu] = new ActionMap();
            actionMaps[(int)Action.BackMenu].keyboardKeys.Add(
                Keys.Escape);
            actionMaps[(int)Action.BackMenu].gamePadButtons.Add(
                Buttons.Back);

            actionMaps[(int)Action.AltMenu] = new ActionMap();
            actionMaps[(int)Action.AltMenu].keyboardKeys.Add(
                Keys.Tab);
            actionMaps[(int)Action.AltMenu].gamePadButtons.Add(
                Buttons.Y);

            actionMaps[(int)Action.Ok] = new ActionMap();
            actionMaps[(int)Action.Ok].keyboardKeys.Add(
                Keys.Enter);
            actionMaps[(int)Action.Ok].gamePadButtons.Add(
                Buttons.A);

            actionMaps[(int)Action.Back] = new ActionMap();
            actionMaps[(int)Action.Back].keyboardKeys.Add(
                Keys.Escape);
            actionMaps[(int)Action.Back].gamePadButtons.Add(
                Buttons.B);            

            actionMaps[(int)Action.CursorUp] = new ActionMap();
            actionMaps[(int)Action.CursorUp].keyboardKeys.Add(
                Keys.Up);
            actionMaps[(int)Action.CursorUp].gamePadButtons.Add(
                Buttons.DPadUp);
            actionMaps[(int)Action.CursorUp].gamePadButtons.Add(
                Buttons.LeftThumbstickUp);

            actionMaps[(int)Action.CursorDown] = new ActionMap();
            actionMaps[(int)Action.CursorDown].keyboardKeys.Add(
                Keys.Down);
            actionMaps[(int)Action.CursorDown].gamePadButtons.Add(
                Buttons.DPadDown);
            actionMaps[(int)Action.CursorDown].gamePadButtons.Add(
                Buttons.LeftThumbstickDown);

            actionMaps[(int)Action.DecreaseAmount] = new ActionMap();
            actionMaps[(int)Action.DecreaseAmount].keyboardKeys.Add(
                Keys.Left);
            actionMaps[(int)Action.DecreaseAmount].gamePadButtons.Add(
                Buttons.DPadLeft);
            actionMaps[(int)Action.DecreaseAmount].gamePadButtons.Add(
                Buttons.LeftThumbstickLeft);

            actionMaps[(int)Action.IncreaseAmount] = new ActionMap();
            actionMaps[(int)Action.IncreaseAmount].keyboardKeys.Add(
                Keys.Right);
            actionMaps[(int)Action.IncreaseAmount].gamePadButtons.Add(
                Buttons.DPadRight);
            actionMaps[(int)Action.IncreaseAmount].gamePadButtons.Add(
                Buttons.LeftThumbstickRight);

            actionMaps[(int)Action.PageLeft] = new ActionMap();
            actionMaps[(int)Action.PageLeft].keyboardKeys.Add(
                Keys.LeftShift);
            actionMaps[(int)Action.PageLeft].gamePadButtons.Add(
                Buttons.LeftShoulder);
            actionMaps[(int)Action.PageLeft].gamePadButtons.Add(
                Buttons.LeftTrigger);

            actionMaps[(int)Action.PageRight] = new ActionMap();
            actionMaps[(int)Action.PageRight].keyboardKeys.Add(
                Keys.RightShift);
            actionMaps[(int)Action.PageRight].gamePadButtons.Add(
                Buttons.RightShoulder);
            actionMaps[(int)Action.PageRight].gamePadButtons.Add(
                Buttons.RightTrigger);

            actionMaps[(int)Action.Dpad] = new ActionMap();
            actionMaps[(int)Action.Dpad].gamePadButtons.Add(
                Buttons.DPadUp);
            actionMaps[(int)Action.Dpad].gamePadButtons.Add(
                Buttons.DPadDown);
            actionMaps[(int)Action.Dpad].gamePadButtons.Add(
                Buttons.DPadLeft);
            actionMaps[(int)Action.Dpad].gamePadButtons.Add(
                Buttons.DPadRight);

            actionMaps[(int)Action.WASDKeys] = new ActionMap();
            actionMaps[(int)Action.WASDKeys].keyboardKeys.Add(
                Keys.W);
            actionMaps[(int)Action.WASDKeys].keyboardKeys.Add(
                Keys.A);
            actionMaps[(int)Action.WASDKeys].keyboardKeys.Add(
                Keys.S);
            actionMaps[(int)Action.WASDKeys].keyboardKeys.Add(
                Keys.D);

            actionMaps[(int)Action.ArrowKeys] = new ActionMap();
            actionMaps[(int)Action.WASDKeys].keyboardKeys.Add(
                Keys.Up);
            actionMaps[(int)Action.WASDKeys].keyboardKeys.Add(
                Keys.Left);
            actionMaps[(int)Action.WASDKeys].keyboardKeys.Add(
                Keys.Down);
            actionMaps[(int)Action.WASDKeys].keyboardKeys.Add(
                Keys.Right);

            actionMaps[(int)Action.LeftThumbStick] = new ActionMap();
            actionMaps[(int)Action.LeftThumbStick].gamePadButtons.Add(
                Buttons.LeftThumbstickUp);
            actionMaps[(int)Action.LeftThumbStick].gamePadButtons.Add(
                Buttons.LeftThumbstickDown);
            actionMaps[(int)Action.LeftThumbStick].gamePadButtons.Add(
                Buttons.LeftThumbstickLeft);
            actionMaps[(int)Action.LeftThumbStick].gamePadButtons.Add(
                Buttons.LeftThumbstickRight);

            actionMaps[(int)Action.RightThumbStick] = new ActionMap();
            actionMaps[(int)Action.RightThumbStick].gamePadButtons.Add(
                Buttons.RightThumbstickUp);
            actionMaps[(int)Action.RightThumbStick].gamePadButtons.Add(
                Buttons.RightThumbstickDown);
            actionMaps[(int)Action.RightThumbStick].gamePadButtons.Add(
                Buttons.RightThumbstickLeft);
            actionMaps[(int)Action.RightThumbStick].gamePadButtons.Add(
                Buttons.RightThumbstickRight);
        }


        /// <summary>
        /// Check if an action has been pressed.
        /// </summary>
        public static bool IsActionPressed(Action action)
        {
            return IsActionMapPressed(actionMaps[(int)action]);
        }

        /// <summary>
        /// Check if an action was just performed in the most recent update.
        /// </summary>
        public static bool IsActionTriggered(Action action)
        {
            return IsActionMapTriggered(actionMaps[(int)action]);
        }


        /// <summary>
        /// Check if an action map has been pressed.
        /// </summary>
        private static bool IsActionMapPressed(ActionMap actionMap)
        {
            for (int i = 0; i < actionMap.keyboardKeys.Count; i++)
            {
                if (IsKeyPressed(actionMap.keyboardKeys[i]))
                {
                    return true;
                }
            }
            if (CurrentGamePadState.IsConnected)
            {
                for (int i = 0; i < actionMap.gamePadButtons.Count; i++)
                {
                    if (IsGamePadButtonPressed(actionMap.gamePadButtons[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Check if an action map has been triggered this frame.
        /// </summary>
        private static bool IsActionMapTriggered(ActionMap actionMap)
        {
            for (int i = 0; i < actionMap.keyboardKeys.Count; i++)
            {
                if (IsKeyTriggered(actionMap.keyboardKeys[i]))
                {
                    return true;
                }
            }
            if (CurrentGamePadState.IsConnected)
            {
                for (int i = 0; i < actionMap.gamePadButtons.Count; i++)
                {
                    if (IsGamePadButtonTriggered(actionMap.gamePadButtons[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion // Action Mapping


        #region Initialization

        /// <summary>
        /// Initializes the default control keys for all actions.
        /// </summary>
        public static void Initialize()
        {
            for (int i = 0; i < MaxInputs; i++)
            {
                HeldGamePadButtons[i] = new Dictionary<Buttons, float>();
            }
            ResetActionMaps();
        }


        #endregion


        #region Update

        /// <summary>
        /// Updates the keyboard and gamepad control states.
        /// </summary>
        public static void Update(float elapsedTime)
        {
            // update the mouse state
            PreviouseMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();

            // update the keyboard and gamepad states
            for (int i = 0; i < MaxInputs; i++)
            {
                PlayerIndex playerIndex = (PlayerIndex)i;

                PreviousKeyboardStates[i] = CurrentKeyboardStates[i];
                CurrentKeyboardStates[i] = Keyboard.GetState(playerIndex);

                PreviousGamePadStates[i] = CurrentGamePadStates[i];
                CurrentGamePadStates[i] = GamePad.GetState(playerIndex);

                if (CurrentGamePadStates[i].IsConnected)
                {
                    GamePadWasConnected[i] = true;
                }

                //Update ButtonsHeldTime
                HeldGamePadButtonsTime[i] += elapsedTime;

                for (int buttonIndex = 0; buttonIndex < buttons.Length; buttonIndex++)
                {
                    var button = buttons[buttonIndex];

                    if (IsGamePadButtonTriggered(button))
                    {
                        if (HeldGamePadButtons[i].ContainsKey(button))
                        {
                            HeldGamePadButtons[i][button] = HeldGamePadButtonsTime[i];
                        }
                        else
                        {
                            HeldGamePadButtons[i].Add(button, HeldGamePadButtonsTime[i]);
                        }
                    }

                    if (IsGamePadButtonReleased(button))
                    {
                        HeldGamePadButtons[i].Remove(button);
                    }

                    if (HeldGamePadButtons[i].Count == 0)
                    {
                        HeldGamePadButtonsTime[i] = 0;
                    }
                }
            }

        }

        #endregion // Update
    
    }
}
