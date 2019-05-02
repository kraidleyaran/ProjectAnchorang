using Assets.Scripts.System;
using MessageBusLib;
using UnityEngine;

namespace Assets.Scripts.Auras
{
    [CreateAssetMenu(fileName = "Input Control Aura", menuName = @"Anchorang/Aura/Input Control", order = 0)]
    public class InputControlAura : Aura
    {
        private bool _preventInput { get; set; }

        public override void SubscribeController(AuraController controller)
        {
            base.SubscribeController(controller);
            //_controller.gameObject.SendMessage(new RegisterObjectForInputMessage{Object = _controller.transform.parent.gameObject});
            _controller.gameObject.Subscribe<InputStateMessage>(InputState);
            _controller.gameObject.Subscribe<UpdateInputStatusMessage>(UpdateInputStatus);
            _controller.gameObject.SendMessage(new RequestInputStatusMessage());
        }

        private void InputState(InputStateMessage msg)
        {
            if (!_preventInput)
            {
                var moveDirection = msg.CurrentState.LeftStick;
                _controller.gameObject.SendMessageTo(new SetMovementDirectionMessage { Direction = moveDirection },
                    _controller.transform.parent.gameObject);


                var faceDirection = Vector2.zero;
                var aimDirection = Vector2.zero;
                if (msg.CurrentState.RightStick != Vector2.zero)
                {
                    var naturalRightStick = msg.CurrentState.RightStick.NatrualValues();

                    if (naturalRightStick.y > naturalRightStick.x)
                    {
                        if (msg.CurrentState.RightStick.y > 0)
                        {
                            faceDirection.y = 1;
                        }
                        else if (msg.CurrentState.RightStick.y < 0)
                        {
                            faceDirection.y -= 1;
                        }

                    }
                    else if (naturalRightStick.x > naturalRightStick.y)
                    {
                        if (msg.CurrentState.RightStick.x > 0)
                        {
                            faceDirection.x = 1;
                        }
                        else if (msg.CurrentState.RightStick.x < 0)
                        {
                            faceDirection.x -= 1;
                        }
                    }
                    aimDirection = msg.CurrentState.RightStick;
                }
                else
                {
                    faceDirection = moveDirection;
                    aimDirection = moveDirection;
                }
                if (faceDirection != Vector2.zero)
                {
                    _controller.gameObject.SendMessageTo(new SetFacingDirectionMessage { Direction = faceDirection },
                        _controller.transform.parent.gameObject);
                }
                if (aimDirection != Vector2.zero)
                {
                    _controller.gameObject.SendMessageTo(new SetAimDirectionMessage { Direction = aimDirection }, _controller.transform.parent.gameObject);
                }
            }
        }

        private void UpdateInputStatus(UpdateInputStatusMessage msg)
        {
            _preventInput = !msg.AllowInput;
        }

        public override void Destroy()
        {
            base.Destroy();
            //this.SendMessage(new UnregisterObjectForInputMessage { Object = _controller.transform.parent.gameObject });
        }
    }
}