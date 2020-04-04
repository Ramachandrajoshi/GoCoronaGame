
cc.Class({
    extends: cc.Component,

    properties: {
       radious:100
    },

    getBuddusDistence(){
      var player = this.game.player.getPosition()
     var dist= this.node.position.sub(player).mag()
     return dist
    },
    onPicked(){
        this.game.placeStar()
        this.node.destroy()
    },

    update (dt) {
        var rad = this.getBuddusDistence()
        if( rad < this.radious){
            this.onPicked()
            return
        }
        var opacityRatio = 1 - this.game.timer / this.game.starDuration;
        var minOpacity = 50
        this.node.opacity = minOpacity + Math.floor(opacityRatio * (255 - minOpacity))
    },
});
