package com.cartify.cartify.controller;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

/**
 * Controller that maps custom error pages for Cartify.
 *
 * <p>Spring MVC needs an explicit handler for {@code /error/403} so that
 * Thymeleaf can resolve and render {@code templates/error/403.html}.
 * Without this mapping, the redirect from {@link com.cartify.cartify.config.CustomAccessDeniedHandler}
 * would hit Spring Boot's default whitelabel error page instead of our styled page.
 *
 * <p>The {@code /error/**} path is explicitly allowed for all users in
 * {@link com.cartify.cartify.config.SecurityConfig} so that even authenticated
 * users who are denied access can reach this page without getting into a redirect loop.
 */
@Controller
@RequestMapping("/error")
public class ErrorController {

    /**
     * Renders the 403 Access Denied page.
     *
     * @return the Thymeleaf template name {@code error/403}
     *         (resolves to {@code templates/error/403.html})
     */
    @GetMapping("/403")
    public String accessDenied() {
        return "error/403";
    }
}
