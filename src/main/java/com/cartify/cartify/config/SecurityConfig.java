package com.cartify.cartify.config;

import com.cartify.cartify.service.CustomUserDetailsService;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.dao.DaoAuthenticationProvider;
import org.springframework.security.config.annotation.authentication.configuration.AuthenticationConfiguration;
import org.springframework.security.config.annotation.method.configuration.EnableMethodSecurity;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.config.Customizer;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.security.web.SecurityFilterChain;

/**
 * Production-ready Spring Security 6 configuration for Cartify E-Commerce.
 * Uses SecurityFilterChain bean (no WebSecurityConfigurerAdapter).
 * Enables role-based authorization, BCrypt encoding, and CSRF protection.
 */
@Configuration
@EnableWebSecurity
@EnableMethodSecurity(prePostEnabled = true)
public class SecurityConfig {

    private final CustomUserDetailsService userDetailsService;

    public SecurityConfig(CustomUserDetailsService userDetailsService) {
        this.userDetailsService = userDetailsService;
    }

    /**
     * Defines the security filter chain: public vs protected URLs and login/logout behaviour.
     */
    @Bean
    public SecurityFilterChain securityFilterChain(HttpSecurity http) throws Exception {
        http
                // ----- CSRF: Enabled by default for form-based login and Thymeleaf forms -----
                .csrf(Customizer.withDefaults())

                // ----- Authorization: Define which URLs are public vs restricted -----
                .authorizeHttpRequests(auth -> auth
                        // Public: home, register, login, static resources
                        .requestMatchers("/", "/home", "/register", "/login").permitAll()
                        .requestMatchers("/css/**", "/js/**", "/images/**").permitAll()
                        // Admin area: only users with role ADMIN
                        .requestMatchers("/admin/**").hasRole("ADMIN")
                        // Cart: only CUSTOMER role
                        .requestMatchers("/cart/**").hasRole("CUSTOMER")
                        // Orders: any authenticated user
                        .requestMatchers("/orders/**").authenticated()
                        // All other requests require authentication
                        .anyRequest().authenticated()
                )

                // ----- Form login: custom page and success handling -----
                .formLogin(form -> form
                        .loginPage("/login")
                        .permitAll()
                        .successHandler((request, response, authentication) -> {
                            String redirectUrl = "/home";
                            if (authentication.getAuthorities().stream()
                                    .anyMatch(a -> "ROLE_ADMIN".equals(a.getAuthority()))) {
                                redirectUrl = "/admin/dashboard";
                            }
                            response.sendRedirect(request.getContextPath() + redirectUrl);
                        })
                )

                // ----- Logout -----
                .logout(logout -> logout
                        .logoutUrl("/logout")
                        .logoutSuccessUrl("/login?logout")
                        .invalidateHttpSession(true)
                        .deleteCookies("JSESSIONID")
                        .permitAll()
                );

        return http.build();
    }

    /**
     * Authentication provider that uses CustomUserDetailsService and BCrypt.
     */
    @Bean
    public DaoAuthenticationProvider authenticationProvider() {
        DaoAuthenticationProvider provider = new DaoAuthenticationProvider(userDetailsService);
        provider.setPasswordEncoder(passwordEncoder());
        return provider;
    }

    /**
     * BCrypt password encoder for secure password storage (production standard).
     */
    @Bean
    public PasswordEncoder passwordEncoder() {
        return new BCryptPasswordEncoder();
    }

    @Bean
    public AuthenticationManager authenticationManager(AuthenticationConfiguration config) throws Exception {
        return config.getAuthenticationManager();
    }
}
