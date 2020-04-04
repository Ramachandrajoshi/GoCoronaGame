cc.Class({
    extends: cc.Component,

    properties: {
      jumpHeight: 0,
      jumpDuration: 0,
      maxMoveSpeed: 0,
      accel: 0,
      jumpAudio:{
        default:null,
        type:cc.AudioClip
      },

    },

    getJumpAction:function(){
      var jumpUp = cc.moveBy(this.jumpDuration,cc.v2(0,this.jumpHeight)).easing(cc.easeCubicActionOut())
      var jumpDown = cc.moveBy(this.jumpDuration,cc.v2(0,-this.jumpHeight)).easing(cc.easeCubicActionIn())
      var callback = cc.callFunc(this.playJumpSound,this)
      return cc.repeatForever(cc.sequence(jumpUp,jumpDown,callback))
    },
    playJumpSound(){
      cc.audioEngine.playEffect(this.jumpAudio,false)
    },

    onKeyPress:function(event){
      switch(event.keyCode){
        case cc.macro.KEY.left:
          this.accelLeft = true
          break
          case cc.macro.KEY.right:
          this.accelRight = true
          break
      }
    },
    onKeyUp:function(event){
    
      switch(event.keyCode){
        case cc.macro.KEY.left:
          this.accelLeft = false
          break
          case cc.macro.KEY.right:
          this.accelRight = false
          break
      }
    },
    // use this for initialization
    onLoad: function () {
      this.isDestroyed = false
      this.node.runAction(this.getJumpAction())
      this.accelLeft = false;
      this.accelRight = false;
      this.xSpeed = 0;
      cc.systemEvent.on(cc.SystemEvent.EventType.KEY_DOWN,this.onKeyPress,this)
      cc.systemEvent.on(cc.SystemEvent.EventType.KEY_UP,this.onKeyUp,this)
    },

    // called every frame
    update: function (dt) {
    
      if(this.accelLeft){
        this.xSpeed -= this.accel * dt
      }else if(this.accelRight){
        this.xSpeed += this.accel * dt
      }
      if(Math.abs(this.xSpeed) > this.maxMoveSpeed){
        this.xSpeed = this.maxMoveSpeed * this.xSpeed/Math.abs(this.xSpeed)
      }
      if(!this.isDestroyed){
      this.node.x += this.xSpeed * dt
      }
    },
    onDestroy:function(){
      this.isDestroyed = true
      cc.systemEvent.off(cc.SystemEvent.EventType.KEY_DOWN,this.onKeyPress,this)
      cc.systemEvent.off(cc.SystemEvent.EventType.KEY_UP,this.onKeyUp,this)
    }
});
