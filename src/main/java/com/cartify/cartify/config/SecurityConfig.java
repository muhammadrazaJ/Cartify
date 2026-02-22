package com.cartify.cartify.config;

import com.cartify.cartify.service.CustomUserDetailsService;
import org.springframework.beans.factory.annotation.Value;
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
 *
 * <p>Key features:
 * <ul>
 *   <li>SecurityFilterChain API (no deprecated WebSecurityConfigurerAdapter)</li>
 *   <li>Role-based authorization (ADMIN / CUSTOMER)</li>
 *   <li>Custom login page at /login with CSRF included automatically by Thymeleaf</li>
 *   <li>Role-based post-login redirect via {@link CustomAuthenticationSuccessHandler}</li>
 *   <li>Remember-Me with 7-day token validity (simple hash token strategy)</li>
 *   <li>Secure logout: invalidates session, clears JSESSIONID and remember-me cookies</li>
 *   <li>BCrypt password encoding (strength 10)</li>
 *   <li>Custom 403 page via {@link CustomAccessDeniedHandler}</li>
 * </ul>
 */
@Configuration
@EnableWebSecurity
@EnableMethodSecurity(prePostEnabled = true)
public class SecurityConfig {

    /** Injected from CustomUserDetailsService for remember-me token validation. */
    private final CustomUserDetailsService userDetailsService;

    /** Dedicated handler that redirects ADMIN → /admin/dashboard, CUSTOMER → /home. */
    private final CustomAuthenticationSuccessHandler successHandler;

    /**
     * Redirects authenticated users to /error/403 when they try to access
     * a resource their role does not permit.
     */
    private final CustomAccessDeniedHandler accessDeniedHandler;

    /**
     * Secret key used to sign remember-me tokens.
     * Defined in application.properties as {@code cartify.remember-me.key}.
     * Change this value in production!
     */
    @Value("${cartify.remember-me.key}")
    private String rememberMeKey;

    public SecurityConfig(CustomUserDetailsService userDetailsService,
                          CustomAuthenticationSuccessHandler successHandler,
                          CustomAccessDeniedHandler accessDeniedHandler) {
        this.userDetailsService = userDetailsService;
        this.successHandler = successHandler;
        this.accessDeniedHandler = accessDeniedHandler;
    }

    /**
     * Main security filter chain — defines URL access rules, form login,
     * remember-me, logout, and exception handling behaviour.
     *
     * <p><b>Rule ordering matters:</b> Spring Security evaluates matchers in
     * declaration order and stops at the first match. Specific paths MUST be
     * declared before broader wildcards (e.g. /admin/products/** before /admin/**).
     */
    @Bean
    public SecurityFilterChain securityFilterChain(HttpSecurity http) throws Exception {
        http
                // ----- CSRF: Enabled by default; Thymeleaf automatically includes the token in forms -----
                .csrf(Customizer.withDefaults())

                // ----- Authorization Rules (order: most-specific first) -----
                .authorizeHttpRequests(auth -> auth

                        // ── Public pages ──────────────────────────────────────────────
                        // Anyone (authenticated or not) can visit these pages.
                        .requestMatchers("/", "/home", "/register", "/login").permitAll()

                        // ── Static resources ──────────────────────────────────────────
                        // CSS, JavaScript, and image files never require authentication.
                        .requestMatchers("/css/**", "/js/**", "/images/**").permitAll()

                        // ── Error pages ───────────────────────────────────────────────
                        // Must be permitAll so the redirect from CustomAccessDeniedHandler
                        // can reach the 403 template without triggering a redirect loop.
                        .requestMatchers("/error/**").permitAll()

                        // ── Admin sub-sections: ROLE_ADMIN only ───────────────────────
                        // Declared explicitly (before the /admin/** catch-all) for clarity
                        // and to make it easy to add finer-grained rules per sub-section.
                        .requestMatchers("/admin/products/**").hasRole("ADMIN")
                        .requestMatchers("/admin/orders/**").hasRole("ADMIN")
                        .requestMatchers("/admin/users/**").hasRole("ADMIN")

                        // ── Admin catch-all: ROLE_ADMIN only ─────────────────────────
                        // Covers /admin/dashboard and any other admin path not listed above.
                        .requestMatchers("/admin/**").hasRole("ADMIN")

                        // ── Cart: ROLE_CUSTOMER only ──────────────────────────────────
                        // Only customers can add to / view / modify their shopping cart.
                        .requestMatchers("/cart/**").hasRole("CUSTOMER")

                        // ── Orders: any authenticated user ────────────────────────────
                        // Both ADMIN and CUSTOMER can view order-related pages.
                        .requestMatchers("/orders/**").authenticated()

                        // ── Everything else requires authentication ────────────────────
                        .anyRequest().authenticated()
                )

                // ----- Form Login -----
                .formLogin(form -> form
                        .loginPage("/login")               // Custom Thymeleaf login page
                        .usernameParameter("username")     // Maps to the email field (name="username")
                        .passwordParameter("password")
                        .successHandler(successHandler)    // Role-based redirect after successful login
                        .failureUrl("/login?error")        // Add ?error param so Thymeleaf shows the error banner
                        .permitAll()
                )

                // ----- Remember-Me (7-day simple hash token) -----
                .rememberMe(remember -> remember
                        .rememberMeParameter("remember-me")           // Must match checkbox name in login.html
                        .tokenValiditySeconds(7 * 24 * 60 * 60)       // 7 days in seconds
                        .key(rememberMeKey)                           // Secret key from application.properties
                        .userDetailsService(userDetailsService)        // Reload user on remembered token
                        .rememberMeCookieName("cartify-remember-me")  // Descriptive cookie name
                        .useSecureCookie(false)                        // Set true when running over HTTPS in prod
                )

                // ----- Logout -----
                .logout(logout -> logout
                        .logoutUrl("/logout")                          // POST /logout triggers logout
                        .logoutSuccessUrl("/login?logout")             // Banner shown via th:if="${param.logout}"
                        .invalidateHttpSession(true)                   // Destroy server-side session
                        .clearAuthentication(true)                     // Remove SecurityContext
                        .deleteCookies("JSESSIONID", "cartify-remember-me") // Wipe both cookies
                        .permitAll()
                )

                // ----- Exception Handling -----
                // When an authenticated user is denied access (403), redirect to
                // the custom Thymeleaf error page instead of showing a blank browser error.
                .exceptionHandling(ex -> ex
                        .accessDeniedHandler(accessDeniedHandler)
                );

        return http.build();
    }

    /**
     * Authentication provider backed by JPA + BCrypt.
     * Spring Security auto-discovers this bean and wires it to the filter chain.
     */
    @Bean
    public DaoAuthenticationProvider authenticationProvider() {
        DaoAuthenticationProvider provider = new DaoAuthenticationProvider(userDetailsService);
        provider.setPasswordEncoder(passwordEncoder());
        return provider;
    }

    /**
     * BCrypt password encoder — industry-standard for storing user passwords.
     * Default strength = 10 (≈100 ms per hash on modern hardware, adequate for prod).
     */
    @Bean
    public PasswordEncoder passwordEncoder() {
        return new BCryptPasswordEncoder();
    }

    /**
     * Exposes the AuthenticationManager as a bean so controllers/services
     * can programmatically authenticate if needed (e.g., auto-login after registration).
     */
    @Bean
    public AuthenticationManager authenticationManager(AuthenticationConfiguration config) throws Exception {
        return config.getAuthenticationManager();
    }
}
