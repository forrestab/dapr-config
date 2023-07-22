CREATE TRIGGER notify_event
    AFTER INSERT OR UPDATE OR DELETE 
        ON app_settings
    FOR EACH ROW 
        EXECUTE PROCEDURE configuration_event();
