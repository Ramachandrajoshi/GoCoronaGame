
cc.Class({
    extends: cc.Component,

    properties: {
       starPrefab:{
           default:null,
           type:cc.Prefab
       },maxStarsDuration:0,
       minStarsDuration:0,
       currentScore:-1,
       ground:{
           default:null,
           type:cc.Node
       },
       player:{
           default:null,
           type:cc.Node
       },
       scoreLabel:{
        default:null,
        type:cc.Label
    },
    capturedAudio:{
        default:null,
        type:cc.AudioClip
      },

    },
    

    // LIFE-CYCLE CALLBACKS:

    onLoad () {
        this.currentScore = -1
        this.timer = 0
        this.starDuration = 10
        this.groundY = this.ground.y + this.ground.height/2;
        this.placeStar()
    },

    placeStar(){
        this.increaseScore()
        var newStar = cc.instantiate(this.starPrefab)
        newStar.getComponent("Star").game = this
        this.node.addChild(newStar)
        var pos = this.getStarPos()
        newStar.setPosition(pos)
        this.starDuration = this.minStarsDuration + Math.random() * (this.maxStarsDuration - this.minStarsDuration)
        this.timer = 0
    },
    increaseScore(){
        cc.audioEngine.playEffect(this.capturedAudio,false)
        this.currentScore++

        if((this.currentScore%4) == 0){
            this.player.getComponent("Buddu").maxMoveSpeed  += 30
            this.player.getComponent("Buddu").accel += 30
        }
        this.scoreLabel.string = "Score:"+this.currentScore
    },
    getStarPos(){
        var randX = 0
        var randY = this.groundY + Math.random() * this.player.getComponent("Buddu").jumpHeight + 50
        var maxX = this.node.width / 2
        randX = (Math.random() - 0.5) * 2 * maxX
    
        return cc.v2(randX,randY)
    },

    update (dt) {  
        if(this.timer > this.starDuration){
            this.gameOver()
            return
        }
        this.timer += dt
    },
    gameOver(){
        this.player.stopAllActions()
        this.player.getComponent("Buddu").onDestroy()
        //cc.director.loadScene('helloworld')
    }
});
