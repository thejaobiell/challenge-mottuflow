package com.sprint.MottuFlow.infra.security;

import com.sprint.MottuFlow.domain.autenticao.TokenService;
import com.sprint.MottuFlow.domain.funcionario.Funcionario;
import com.sprint.MottuFlow.domain.funcionario.FuncionarioRepository;
import jakarta.servlet.FilterChain;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Component;
import org.springframework.web.filter.OncePerRequestFilter;

import java.io.IOException;

@Component
public class FiltroTokenAcesso extends OncePerRequestFilter {
	
	private final TokenService tokenService;
	private final FuncionarioRepository funcionarioRepository;
	
	public FiltroTokenAcesso( TokenService tokenService, FuncionarioRepository funcionarioRepository ) {
		this.tokenService = tokenService;
		this.funcionarioRepository = funcionarioRepository;
	}
	
	@Override
	protected void doFilterInternal( HttpServletRequest request, HttpServletResponse response, FilterChain filterChain ) throws ServletException, IOException {

		String token = recuperarTokenRequisicao( request );
		
		if ( token != null ) {
			try {
				String usuario = tokenService.getSubject( token );
				Funcionario funcionario = funcionarioRepository.findByEmailIgnoreCase( usuario )
						.orElseThrow( () -> new RuntimeException( "Usuário não encontrado" ) );
				
				Authentication authentication = new UsernamePasswordAuthenticationToken( funcionario, null, funcionario.getAuthorities() );
				SecurityContextHolder.getContext().setAuthentication( authentication );
				
			} catch ( Exception e ) {}
		}
		filterChain.doFilter( request, response );
	}
	
	private String recuperarTokenRequisicao( HttpServletRequest request ) {
		String authHeader = request.getHeader( "Authorization" );
		if ( authHeader != null && authHeader.startsWith( "Bearer " ) ) {
			return authHeader.substring( 7 );
		}
		return null;
	}
}
