package com.cartify.cartify.config;

import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.web.access.AccessDeniedHandler;
import org.springframework.stereotype.Component;

import java.io.IOException;

/**
 * Custom access-denied handler for Cartify.
 *
 * <p>Triggered by Spring Security whenever an authenticated user attempts to
 * access a resource they are not authorised to view (HTTP 403). Instead of
 * showing a blank browser error, this handler:
 * <ol>
 *   <li>Logs a WARN entry with the username and the URL they tried to reach.</li>
 *   <li>Redirects the browser to the friendly Thymeleaf 403 page at {@code /error/403}.</li>
 * </ol>
 *
 * <p>Wired into the security filter chain via
 * {@code .exceptionHandling(ex -> ex.accessDeniedHandler(...))} in
 * {@link SecurityConfig}.
 */
@Component
public class CustomAccessDeniedHandler implements AccessDeniedHandler {

    private static final Logger log = LoggerFactory.getLogger(CustomAccessDeniedHandler.class);

    /**
     * Called by Spring Security when an {@link AccessDeniedException} is thrown.
     *
     * @param request   the incoming HTTP request
     * @param response  the outgoing HTTP response
     * @param exception the access-denied exception (not used beyond logging)
     * @throws IOException if the redirect fails
     */
    @Override
    public void handle(HttpServletRequest request,
                       HttpServletResponse response,
                       AccessDeniedException exception) throws IOException {

        // ---- Log who was denied and what URL they tried to reach ----
        Authentication auth = SecurityContextHolder.getContext().getAuthentication();
        String username = (auth != null) ? auth.getName() : "anonymous";
        String requestedUrl = request.getRequestURI();

        log.warn("Access denied: user='{}' attempted to access '{}'", username, requestedUrl);

        // ---- Redirect to the custom 403 Thymeleaf page ----
        // We forward the original URL as a query param so the page can display it.
        response.sendRedirect(request.getContextPath() + "/error/403");
    }
}
