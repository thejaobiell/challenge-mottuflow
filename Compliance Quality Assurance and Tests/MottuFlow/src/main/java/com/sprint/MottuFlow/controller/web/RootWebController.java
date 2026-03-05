package com.sprint.MottuFlow.controller.web;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

//este controller serve apenas para redirecionar para a pagína de login assim que acessar o localhost:8080
@Controller
public class RootWebController {
	@GetMapping("/")
	public String redirectToLogin() {
		return "redirect:/login";
	}
}
