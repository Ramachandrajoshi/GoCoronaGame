cc.Class({
  extends: cc.Component,

  properties: {
    starPrefab: {
      default: null,
      type: cc.Prefab,
    },
    maxStarsDuration: 0,
    minStarsDuration: 0,
    currentScore: -1,
    ground: {
      default: null,
      type: cc.Node,
    },
    player: {
      default: null,
      type: cc.Node,
    },
    scoreLabel: {
      default: null,
      type: cc.Label,
    },
    rightSanitizer: {
      default: null,
      type: cc.Sprite,
    },
    leftSanitizer: {
      default: null,
      type: cc.Sprite,
    },
    startingOverlay: {
      default: null,
      type: cc.Node,
    },
    instructionLabel: {
      default: null,
      type: cc.Label,
    },
    playButton: {
      default: null,
      type: cc.Node,
    },
    tryAgainButton: {
        default: null,
        type: cc.Node,
      },
    capturedAudio: {
      default: null,
      type: cc.AudioClip,
    },
  },

  // LIFE-CYCLE CALLBACKS:

  onLoad() {
    this.groundY = this.ground.y + this.ground.height / 2;
    this.tryAgainButton.runAction(cc.hide());
  },

  playGame() {
    if (this.isGameOver) {
      cc.director.loadScene("helloworld");
    } else {
      this.startingOverlay.runAction(cc.hide());
      this.isGameOver = false;
      this.currentScore = -1;
      this.timer = 0;
      this.starDuration = 10;
      this.player.getComponent("Doc").onPlayClicked();
      this.placeStar();
    }
  },

  placeStar() {
    this.increaseScore();
    var newStar = cc.instantiate(this.starPrefab);
    newStar.getComponent("Corona").game = this;
    this.node.addChild(newStar);
    var pos = this.getStarPos();
    newStar.setPosition(pos);
    this.starDuration =
      this.minStarsDuration +
      Math.random() * (this.maxStarsDuration - this.minStarsDuration);
    this.timer = 0;
  },
  increaseScore() {
    cc.audioEngine.playEffect(this.capturedAudio, false);
    this.currentScore++;
    this.rotateSanitizer();
    if (this.currentScore % 4 == 0) {
      this.player.getComponent("Doc").maxMoveSpeed += 30;
      this.player.getComponent("Doc").accel += 30;
    }
    this.scoreLabel.string = "Score:" + this.currentScore;
  },
  rotateSanitizer() {
    var rotRight = cc.rotateBy(1, 360).easing(cc.easeElasticIn());
    var rotLeft = cc.rotateBy(1, -360).easing(cc.easeElasticIn());
    this.rightSanitizer.node.runAction(cc.repeat(rotRight, 1));
    this.leftSanitizer.node.runAction(cc.repeat(rotLeft, 1));
  },
  getStarPos() {
    var randX = 0;
    var randY =
      this.groundY +
      Math.random() * this.player.getComponent("Doc").jumpHeight +
      50;
    var maxX = this.node.width / 2;
    randX = (Math.random() - 0.5) * 2 * maxX;

    return cc.v2(randX, randY);
  },

  update(dt) {
    if (this.timer > this.starDuration) {
      this.gameOver();
      return;
    }
    this.timer += dt;
  },
  gameOver() {
    this.player.stopAllActions();
    this.player.getComponent("Doc").onDestroy();
    this.instructionLabel.string = "Game Over";
    this.instructionLabel.fontSize = 28;
    this.isGameOver = true;
    this.playButton.runAction(cc.hide());
    this.tryAgainButton.runAction(cc.show());
    this.startingOverlay.runAction(cc.show());
  },
});
