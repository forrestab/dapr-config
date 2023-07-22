CREATE OR REPLACE FUNCTION configuration_event() RETURNS TRIGGER AS $$
    DECLARE 
        data json;
        notification json;
    
    BEGIN

        IF (TG_OP = 'DELETE') THEN
            data = row_to_json(OLD);
        ELSE
            data = row_to_json(NEW);
        END IF;
        
        notification = json_build_object(
            'table', TG_TABLE_NAME,
            'action', TG_OP,
            'data', data
        );

        PERFORM pg_notify('config', notification::text);

        RETURN NULL; 
        
    END;  
$$ LANGUAGE plpgsql;
