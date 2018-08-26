﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

	private const string animatorTransition = "movingHorizontally";
	Player _player;
	Animator _anim;

	void Start()
	{
		_player = GetComponentInParent<Player>();
		_anim = GetComponent<Animator>();
	}

	void Update()
	{
		if(GameManager.Instance.gameOver || CameraFollow.shaking){
			_anim.SetBool(animatorTransition, false);
			return;
		}

		if(Input.GetAxisRaw("Horizontal") != 0)
			_anim.SetBool(animatorTransition, true);
		else
			_anim.SetBool(animatorTransition, false);

		// _anim.SetBool("jump", Mathf.Abs(_player.velocity.y) >= 2);
		_anim.SetBool("jump", _player.movementController.collisionInfo.below);
		_anim.SetFloat("velocityY", _player.velocity.y);
	}

}
