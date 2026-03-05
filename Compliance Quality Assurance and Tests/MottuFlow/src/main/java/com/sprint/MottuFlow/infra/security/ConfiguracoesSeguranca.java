package com.sprint.MottuFlow.infra.security;

import org.springframework.context.annotation.*;
import org.springframework.core.annotation.Order;
import org.springframework.security.access.hierarchicalroles.RoleHierarchy;
import org.springframework.security.access.hierarchicalroles.RoleHierarchyImpl;
import org.springframework.security.authentication.*;
import org.springframework.security.config.annotation.authentication.configuration.AuthenticationConfiguration;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.config.annotation.web.configurers.AbstractHttpConfigurer;
import org.springframework.security.config.http.*;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.security.web.*;
import org.springframework.context.annotation.Bean;
import org.springframework.security.web.authentication.UsernamePasswordAuthenticationFilter;

@Configuration
@EnableWebSecurity
public class ConfiguracoesSeguranca {
	
	private final FiltroTokenAcesso filtroTokenAcesso;
	
	public ConfiguracoesSeguranca( FiltroTokenAcesso filtroTokenAcesso ) {
		this.filtroTokenAcesso = filtroTokenAcesso;
	}
	
	// ------------------ API REST ------------------
	@Bean
	@Order(1)
	public SecurityFilterChain apiRestSecurity(HttpSecurity http) throws Exception {
		return http
				.securityMatcher("/api/**")
				.csrf(AbstractHttpConfigurer::disable)
				.sessionManagement(sm -> sm.sessionCreationPolicy(SessionCreationPolicy.STATELESS))
				.authorizeHttpRequests(auth -> auth
						.requestMatchers("/api/login", "/api/atualizar-token", "/api/verificar-jwt").permitAll()
						.anyRequest().authenticated()
				)
				.addFilterBefore(filtroTokenAcesso, UsernamePasswordAuthenticationFilter.class)
				.build();
	}
	
	// ------------------ Web / Thymeleaf ------------------
	@Bean
	@Order(2)
	public SecurityFilterChain webSecurity(HttpSecurity http) throws Exception {
		return http
				.authorizeHttpRequests(auth -> auth
						.requestMatchers("/css/**", "/js/**", "/images/**").permitAll()
						.requestMatchers("/swagger-ui/**", "/v3/api-docs/**").permitAll()
						.requestMatchers("/login").permitAll()
						.requestMatchers("/", "/menu", "/perfil").authenticated()
						.requestMatchers("/motos/**", "/arucotags/**", "/localidades/**", "/status/**")
						.hasRole("MECANICO")
						.anyRequest().hasAnyRole("GERENTE", "ADMIN")
				)
				.formLogin(form -> form
						.loginPage("/login")
						.defaultSuccessUrl("/menu", true)
						.permitAll()
				)
				.logout(logout -> logout
						.logoutUrl("/logout")
						.logoutSuccessUrl("/login?logout")
						.permitAll()
				)
				.exceptionHandling(exception -> exception
						.accessDeniedPage("/403")
				)
				.csrf(csrf -> csrf.ignoringRequestMatchers("/api/**"))
				.build();
	}
	
	@Bean
	public PasswordEncoder encriptador() {
		return new BCryptPasswordEncoder();
	}
	
	@Bean
	public AuthenticationManager authenticationManager(AuthenticationConfiguration authConfig) throws Exception {
		return authConfig.getAuthenticationManager();
	}
	
	@Bean
	public RoleHierarchy hierarchyPerfis() {
		String hierarquia = "ROLE_ADMIN > ROLE_GERENTE \n ROLE_GERENTE > ROLE_MECANICO";
		RoleHierarchyImpl roleHierarchy = new RoleHierarchyImpl();
		roleHierarchy.setHierarchy( hierarquia );
		return roleHierarchy;
	}
}