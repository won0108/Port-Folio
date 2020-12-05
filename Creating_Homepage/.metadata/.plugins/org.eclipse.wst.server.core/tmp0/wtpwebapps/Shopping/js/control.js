(function(){
	var current=0; //현재 위치
	var max=0; //이미지 갯수
	
	var container; //리스트 요소를 감싸고 있는 ul
	var interval;
	<!--이미지 개수 모두 가져온다-->
	function init(){
		container=jQuery(".slide ul");
		max=container.children().length;
		
		events();
		//이미지 자동 넘김 실행
		interval = setInterval(next, 3000);
	}
	//화살표 버튼 클릭 시, 이벤트 시작
	function events(){
		jQuery("button.prev").on("click",prev);
		jQuery("button.next").on("click", next);
		
		jQuery(window).on("keydown", keydown);
	
	}
	//왼쪽 버튼 클릭 시, 왼쪽 이동 및 첫 이미지일 때 마지막 이미지로 이동
	function prev(e){
		current--;
		if(current < 0)current=max-1;
		animate();
		
	}
	//오른쪽 버튼 클릭 시, 오른쪽 이동 및 마지막 이미지일 때 첫 이미지로 이동
	function next(e){
		current++;
		if(current>max-1)current=0;
		animate();
	}
	//이미지 넘김 액션
	function animate(){
		var moveX=current * 1200;
		TweenMax.to(container, 0.8, {marginLeft:-moveX, ease:Expo.easeOut});
		
		clearInterval(interval);
		interval = setInterval(next, 3000);
		
	}
	jQuery(document).ready(init);
})();

